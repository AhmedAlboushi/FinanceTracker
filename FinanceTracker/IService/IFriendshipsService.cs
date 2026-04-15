using FinanceTracker.Dtos.FriendshipDto;

namespace FinanceTracker.IService
{
    public interface IFriendshipsService
    {
        public Task<ICollection<FriendshipDto>> GetFriendsByUserId(int userId);

        public Task<ICollection<FriendshipDto>> GetFriendRequestsByUserId(int userId);

        public Task SendFriendRequest(int userId, int friendId);

        public Task AcceptFriendRequest(int userId, int friendId);

        public Task RejectFriendRequest(int userId, int friendId);
        public Task RemoveFriend(int userId, int friendId);
    }
}
