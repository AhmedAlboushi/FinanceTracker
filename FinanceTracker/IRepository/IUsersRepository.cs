using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IUsersRepository
    {

        Task<ICollection<User>> GetUsers();
        Task<ICollection<User>> GetUsersConnectedToWalletByWalletId(int walletId);

        Task<User?> GetUser(int userId);

        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);

        Task CreateUser(User user);

        Task UpdateUser(User user);


        Task<bool> DoesUserExistByEmail(string email);
        Task<bool> DoesUserExistByUsername(string username);
        Task<bool> DoesUserExist(int userId);





    }
}
