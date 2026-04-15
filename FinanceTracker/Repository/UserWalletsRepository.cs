using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class UserWalletsRepository : IUserWalletsRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public UserWalletsRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserWallet(Userwallet userWallet)
        {
            await _context.Userwallets.AddAsync(userWallet);
           await _context.SaveChangesAsync();
        
        }

        public async Task DeleteUserWallet(int userId, int walletId)
        {
            await _context.Userwallets
                .Where(u => u.Userid == userId && u.Walletid == walletId)
                .ExecuteDeleteAsync();
        }

        public async Task<Userwallet?> GetUserWallet(int userWalletId)
        {
            return await _context.Userwallets.FindAsync(userWalletId);
        }

        public async Task<ICollection<Userwallet>> GetUserWalletsByUserId(int userId)
        {
            return await _context.Userwallets
                .Where(u => u.Userid == userId)
                .ToListAsync();
        }

        public async Task<Userwallet?> GetUserWalletByUserIdAndWalletId(int userId, int walletId)
        {
            return await _context.Userwallets
      .FirstOrDefaultAsync(u => u.Userid == userId && u.Walletid == walletId);
        }

        public async Task<ICollection<Userwallet>> GetUserWalletsByWalletId(int walletId)
        {
            return await _context.Userwallets
                .Include(u => u.User)
            .Where(u => u.Walletid == walletId)
            .ToListAsync();
        }

        public Task UpdateUserWallet(Userwallet userWallet)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateWallet(Userwallet userWallet)
        {
            _context.Userwallets.Update(userWallet);

            await _context.SaveChangesAsync();
        }


    }
}
