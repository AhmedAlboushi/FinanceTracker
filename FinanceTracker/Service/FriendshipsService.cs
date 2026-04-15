using FinanceTracker.Dtos.FriendshipDto;
using FinanceTracker.Enums;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class FriendshipsService : IFriendshipsService
    {
        private readonly IFriendshipsRepository _friendshipsRepository;
        private readonly IUsersRepository _usersRepository;

        public FriendshipsService(IFriendshipsRepository friendshipsRepository, IUsersRepository usersRepository)
        {
            _friendshipsRepository = friendshipsRepository;
            _usersRepository = usersRepository;
        }
        public async Task AcceptFriendRequest(int userId, int friendId)
        {
            var friendShip = await _friendshipsRepository.GetFriendship(userId, friendId);

            if (friendShip == null)
                throw new KeyNotFoundException("Request Doesn't Exist!");

            if (friendShip.Userid == userId)
                throw new KeyNotFoundException("You cannot accept your own friend request!");

            if (friendShip.Status == (byte)FriendshipStatus.Accepted)
                throw new KeyNotFoundException("You are already friends with this user!");

            if (friendShip.Frienduserid == userId && friendShip.Status == (byte)FriendshipStatus.Rejected)
                throw new InvalidOperationException("Cannot accept rejected requests");





            friendShip.Status = (byte)FriendshipStatus.Accepted;

            await _friendshipsRepository.UpdateFriendship(friendShip);
        }

        public async Task<ICollection<FriendshipDto>> GetFriendRequestsByUserId(int userId)
        {
            var friendRequests = await _friendshipsRepository.GetFriendRequestsByUserId(userId);

            return friendRequests.Select(f => new FriendshipDto()
            {
                FriendshipId = f.Friendshipid,
                FriendUsername = f.Frienduser.Userid == userId ? f.User.Username : f.Frienduser.Username,
                FriendUserId = f.Frienduser.Userid == userId ? f.User.Userid : f.Frienduser.Userid,
                CreatedAt = f.Createdat,
            }).ToList();
        }

        public async Task<ICollection<FriendshipDto>> GetFriendsByUserId(int userId)
        {
            var friends = await _friendshipsRepository.GetFriendsByUserId(userId);

            return friends.Select(f => new FriendshipDto()
            {
                FriendshipId = f.Friendshipid,
                // if someone sent the request and i accepted i am in the friend spot so displaying friendUserName is ur name
                // so i check if friendId == my Id then i display the sender name because im the receiver
                FriendUsername = f.Frienduser.Userid == userId ? f.User.Username : f.Frienduser.Username,

                CreatedAt = f.Createdat,

                // same as above incase someone sent me the request i will be in the friend spot so i switch it after the check
                FriendUserId = f.Frienduser.Userid == userId ? f.User.Userid : f.Frienduser.Userid,
            }).ToList();
        }

        public async Task RejectFriendRequest(int userId, int friendId)
        {
            var friendShip = await _friendshipsRepository.GetFriendship(userId, friendId);

            if (friendShip == null)
                throw new KeyNotFoundException("Request Doesn't Exist!");


            // this makes sure that he cannot reject his own request
            if (friendShip.Userid == userId)
                throw new KeyNotFoundException("Cannot reject your own sent request");

            if (friendShip.Status == (byte)FriendshipStatus.Accepted)
                throw new KeyNotFoundException("Cannot reject when you are already friends");


            if (friendShip.Frienduserid == userId && friendShip.Status != (byte)FriendshipStatus.Pending)
                throw new KeyNotFoundException("Cannot reject an already rejected or accepted request");




            friendShip.Status = (byte)FriendshipStatus.Rejected;

            await _friendshipsRepository.DeleteFriendship(friendShip.Friendshipid);
        }

        public async Task RemoveFriend(int userId, int friendId)
        {
            var friendShip = await _friendshipsRepository.GetFriendship(userId, friendId);

            if (friendShip == null)
                throw new KeyNotFoundException("Friend Doesn't Exist!");

            if (friendShip.Status != (byte)FriendshipStatus.Accepted)
                throw new KeyNotFoundException("You aren't friends with this user yet");


            await _friendshipsRepository.DeleteFriendship(friendShip.Friendshipid);
        }

        public async Task SendFriendRequest(int userId, int friendId)
        {
            if (userId == friendId)
                throw new InvalidOperationException("Cannot send friend request to yourself");

            var user = await _usersRepository.GetUser(userId);

            var friend = await _usersRepository.GetUser(friendId);

            if (user == null)
                throw new KeyNotFoundException("User not found");
            if (friend == null)
                throw new KeyNotFoundException("User not found");

            var friendShip = await _friendshipsRepository.GetFriendship(userId, friendId);

            if (friendShip != null)
                throw new InvalidOperationException("There is a pending friend request or you are Already Friends with this user!");

            var friendship = new Friendship()
            {
                Frienduserid = friendId,
                Userid = userId
                ,
                Createdat = DateOnly.FromDateTime(DateTime.UtcNow)
                ,
                Status = (byte)FriendshipStatus.Pending,
            };

            await _friendshipsRepository.CreateFriendship(friendship);
        }
    }
}
