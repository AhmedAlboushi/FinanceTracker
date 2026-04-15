using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IFriendshipsRepository
    {
        public Task<ICollection<Friendship>> GetFriendsByUserId(int userId);
        public Task<ICollection<Friendship>> GetFriendRequestsByUserId(int userId);
        public Task<Friendship> GetFriendship(int userId, int friendId);

        public Task CreateFriendship(Friendship friendship);

        public Task UpdateFriendship(Friendship friendship);
        public Task DeleteFriendship(int friendshipId);
    }
}
