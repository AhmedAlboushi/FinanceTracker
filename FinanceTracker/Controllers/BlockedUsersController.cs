using FinanceTracker.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("GeneralLimiter")]
    public class BlockedUsersController : ControllerBase
    {
        private readonly IBlockedUsersService _blockedUsersService;

        public BlockedUsersController(IBlockedUsersService blockedUsersService)
        {
            _blockedUsersService = blockedUsersService;

        }

        [HttpGet]

        public async Task<IActionResult> GetBlockedUsersByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var blockedUsersDto = await _blockedUsersService.GetBlockedUsersByUserId(userId);

            return Ok(blockedUsersDto);
        }

        [HttpPost("{targetUserId}")]
        public async Task<IActionResult> BlockUser(int targetUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _blockedUsersService.BlockUser(userId, targetUserId);

            return Ok("User Blocked Succesfully!");

        }

        [HttpDelete("{targetUserId}")]

        public async Task<IActionResult> UnBlockUser(int targetUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _blockedUsersService.UnBlockUser(userId, targetUserId);

            return Ok("User Unblocked Succesfully!");
        }


    }
}
