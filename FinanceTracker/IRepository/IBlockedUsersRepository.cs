using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IBlockedUsersRepository
    {
        public Task<ICollection<Blockeduser>> GetBlockedUsersByUserId(int userId);

        public Task<Blockeduser> GetBlockedUser(int userId, int targerUserId);
        public Task CreateBlockedUser(Blockeduser blockedUser);

        public Task<bool> IsBlocked(int userId, int targetUserId);
        public Task DeleteBlockedUser(int userId, int targetUserId);
    }
}
