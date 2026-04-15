using FinanceTracker.Data;
using FinanceTracker.Enums;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class FriendShipsRepository : IFriendshipsRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public FriendShipsRepository(FinanceTrackerDbContext context)

        {
            _context = context;
        }

        public async Task CreateFriendship(Friendship friendship)
        {
            await _context.Friendships.AddAsync(friendship);

            await _context.SaveChangesAsync();

        }

        public async Task DeleteFriendship(int FriendshipId)
        {
            await _context.Friendships
                .Where(f => f.Friendshipid == FriendshipId)
                .ExecuteDeleteAsync();
        }

        public async Task<ICollection<Friendship>> GetFriendRequestsByUserId(int userId)
        {
            return await _context.Friendships
                .Include(f => f.Frienduser)
                .Include(f => f.User)
                .Where(f => f.Frienduserid == userId && f.Status == (byte)FriendshipStatus.Pending)
                 .ToListAsync();
        }

        public async Task<ICollection<Friendship>> GetFriendsByUserId(int userId)
        {
            return await _context.Friendships
                .Include(f => f.Frienduser)
                                .Include(f => f.User)

     .Where(f => (f.Userid == userId || f.Frienduserid == userId)
              && f.Status == (byte)FriendshipStatus.Accepted)
     .ToListAsync();
        }

        public async Task<Friendship> GetFriendship(int userId, int friendId)
        {
            return await _context.Friendships
                         .Where(f => f.Userid == userId && f.Frienduserid == friendId ||
                                     f.Userid == friendId && f.Frienduserid == userId)

                         .FirstOrDefaultAsync();

        }

        public async Task UpdateFriendship(Friendship friendship)
        {
            _context.Friendships.Update(friendship);

            await _context.SaveChangesAsync();

        }
    }
}
