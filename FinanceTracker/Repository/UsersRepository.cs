using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class UsersRepository : IUsersRepository
    {

        private readonly FinanceTrackerDbContext _context;

        public UsersRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }


        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);


            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesUserExist(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Userid == userId);

        }

        public async Task<bool> DoesUserExistByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> DoesUserExistByUsername(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User?> GetUser(int userId)
        {
            return await _context.Users
                  .FindAsync(userId);

        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
              .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users
              .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users
                     .ToListAsync();
        }

        public async Task<ICollection<User>> GetUsersConnectedToWalletByWalletId(int walletId)
        {
            return await _context.Users
                .Include(u => u.Userwallets)
                .Where(u => u.Userwallets.Any(uw => uw.Walletid == walletId))
                .ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}
