using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class BlockedUsersRepository : IBlockedUsersRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public BlockedUsersRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task CreateBlockedUser(Blockeduser blockedUser)
        {
            await _context.Blockedusers.AddAsync(blockedUser);

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Blockeduser>> GetBlockedUsersByUserId(int userId)
        {
            return await _context.Blockedusers
                  .Include(b => b.Targetuser)
                  .Where(b => b.Userid == userId)
                   .ToListAsync();
        }

        public async Task DeleteBlockedUser(int userId, int targetUserId)
        {
            await _context.Blockedusers
       .Where(b => b.Userid == userId && b.Targetuserid == targetUserId)
       .ExecuteDeleteAsync();
        }

        public async Task<bool> IsBlocked(int userId, int targetUserId)
        {
            return await _context.Blockedusers
                 .AnyAsync(b => (b.Userid == userId && b.Targetuserid == targetUserId) ||
                        (b.Userid == targetUserId && b.Targetuserid == userId));
        }

        public async Task<Blockeduser> GetBlockedUser(int userId, int targerUserId)
        {
            return await _context.Blockedusers
                .FirstOrDefaultAsync(b => b.Userid == userId && b.Targetuserid == targerUserId);
        }
    }
}
