using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IUserWalletsRepository
    {

        Task<Userwallet?> GetUserWallet(int userWalletId);
        Task<Userwallet?> GetUserWalletByUserIdAndWalletId(int userId, int walletId);

        Task<ICollection<Userwallet>> GetUserWalletsByUserId(int userId);
        Task<ICollection<Userwallet>> GetUserWalletsByWalletId(int walletId);


        Task UpdateUserWallet(Userwallet userWallet);

        Task DeleteUserWallet(int userId, int walletId);

        Task CreateUserWallet(Userwallet userWallet);

    }
}
