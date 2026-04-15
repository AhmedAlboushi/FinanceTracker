using FinanceTracker.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("GeneralLimiter")]

    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipsService _friendshipsSevice;
        public FriendshipsController(IFriendshipsService friendshipsSevice)

        {
            _friendshipsSevice = friendshipsSevice;
        }


        [HttpGet]

        public async Task<IActionResult> GetFriendsByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var friendsDto = await _friendshipsSevice.GetFriendsByUserId(userId);

            return Ok(friendsDto);
        }

        [HttpGet("requests")]

        public async Task<IActionResult> GetFriendRequestsByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var friendrequestsDto = await _friendshipsSevice.GetFriendRequestsByUserId(userId);

            return Ok(friendrequestsDto);

        }

        [HttpPost("{friendId}/send-request")]
        public async Task<IActionResult> SendFriendRequest(int friendId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _friendshipsSevice.SendFriendRequest(userId, friendId);

            return Ok("Sent friend request succesfully!");
        }

        [HttpPut("{friendId}/accept-request")]

        public async Task<IActionResult> AcceptFriendRequest(int friendId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _friendshipsSevice.AcceptFriendRequest(userId, friendId);

            return Ok("Friend request accepted succesfully!");
        }

        [HttpDelete("{friendId}/reject-request")]


        public async Task<IActionResult> RejectFriendRequest(int friendId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _friendshipsSevice.RejectFriendRequest(userId, friendId);

            return Ok("Rejected request succesfully!");
        }

        [HttpDelete("{friendId}/remove-friend")]

        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _friendshipsSevice.RemoveFriend(userId, friendId);

            return Ok("Friend removed succesfully!");
        }

    }
}
