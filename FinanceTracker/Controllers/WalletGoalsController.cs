using FinanceTracker.Dtos.WalletGoalDto;
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

    public class WalletGoalsController : ControllerBase
    {

        private readonly IWalletGoalsService _walletGoalsService;

        public WalletGoalsController(IWalletGoalsService walletGoalsService)
        {
            _walletGoalsService = walletGoalsService;
        }

        [HttpGet("{walletId}")]

        public async Task<IActionResult> GetGoalsByWalletId(int walletId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var goalsDto = await _walletGoalsService.GetGoalsByWalletId(userId, walletId);

            return Ok(goalsDto);
        }

        [HttpPut("{walletId}/allocate/{goalId}")]

        public async Task<IActionResult> AllocateAmountToGoal(int walletId, int goalId, AllocateAmountToGoalDto walletGoalDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _walletGoalsService.AllocateAmountToGoal(userId, walletId, goalId, walletGoalDto);

            return Ok("Allocated Succesfully!");
        }


        [HttpPost("{walletId}")]

        public async Task<IActionResult> CreateGoal(int walletId, CreateWalletGoalDto walletGoalDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var walletGoalId = await _walletGoalsService.CreateGoal(userId, walletId, walletGoalDto);

            return CreatedAtAction(nameof(GetGoalsByWalletId), new { walletId }, walletGoalId);

        }

        [HttpDelete("{walletId}/{goalId}")]

        public async Task<IActionResult> DeleteGoal(int walletId, int goalId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _walletGoalsService.DeleteGoal(userId, walletId, goalId);

            return Ok("Goal deleted succesfully!");
        }

        [HttpPut("{walletId}/{goalId}")]

        public async Task<IActionResult> UpdateGoal(int walletId, int goalId, UpdateWalletGoalDto walletGoalDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _walletGoalsService.UpdateGoal(userId, walletId, goalId, walletGoalDto);

            return Ok("Goal updated succesfully!");
        }

        [HttpPost("{walletId}/add-image/{goalId}")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> AddImageToGoal(int walletId, int goalId, IFormFile goalImage)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _walletGoalsService.AddImageToGoal(userId, walletId, goalId, goalImage);

            return Ok("Image Added succesfully!");
        }
    }
}
