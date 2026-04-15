using FinanceTracker.Dtos.BlockedUserDto;

namespace FinanceTracker.IService
{
    public interface IBlockedUsersService
    {

        public Task<ICollection<BlockedUserDto>> GetBlockedUsersByUserId(int userId);
        public Task BlockUser(int userId, int targerUserId);

        public Task UnBlockUser(int userId, int targetUserId);

    }
}
