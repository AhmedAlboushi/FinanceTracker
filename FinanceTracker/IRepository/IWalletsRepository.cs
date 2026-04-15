using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IWalletsRepository
    {
        Task<Wallet?> GetWallet(int walletId);
        Task<ICollection<Wallet>> GetWalletsByUserId(int userId);

        Task<Wallet> GetWalletByOwnerRole(int userId);
        Task CreateWallet(Wallet wallet);
        Task UpdateWallet(Wallet wallet);
    }
}
