using FinanceTracker.Data;
using FinanceTracker.Enums;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class WalletsRepository : IWalletsRepository
    {

        private readonly FinanceTrackerDbContext _context;

        public WalletsRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task CreateWallet(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet?> GetWallet(int walletId)
        {
 
            return await _context.Wallets
                                .Include(w => w.Userwallets)

                .FirstOrDefaultAsync(w => w.Walletid == walletId);
        }

        public async Task<Wallet> GetWalletByOwnerRole(int userId)
        {
            return await _context.Wallets
                .Include(w => w.Userwallets)
                .Where(w => w.Userwallets.Any(uw => uw.Userid == userId && uw.Walletroleid == (byte)WalletRoleType.Owner))
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Wallet>> GetWalletsByUserId(int userId)
        {
            return await _context.Wallets
                .Include(w => w.Userwallets)
                .Where(w =>
                    w.Userwallets.Any(uw => uw.Userid == userId))
                    .ToListAsync();

        }

        public async Task UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
