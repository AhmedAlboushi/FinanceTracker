using FinanceTracker.Dtos.IncomeSourceDto;
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

    public class IncomeSourcesController : ControllerBase
    {
        private readonly IIncomeSourcesService _incomeSourcesService;

        public IncomeSourcesController(IIncomeSourcesService incomeSourcesService)
        {
            _incomeSourcesService = incomeSourcesService;
        }

        [HttpGet("{walletId}")]

        public async Task<IActionResult> GetIncomeSourcesByWalletId(int walletId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier!).Value);

            var incomeSourcesDto = await _incomeSourcesService.GetIncomeSourcesByWalletId(userId, walletId);

            return Ok(incomeSourcesDto);

        }

        [HttpPost("{walletId}")]

        public async Task<IActionResult> CreateIncomeSource(int walletId, CreateIncomeSourceDto incomeSourceDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier!).Value);

            await _incomeSourcesService.CreateIncomeSource(userId, walletId, incomeSourceDto);

            return Ok("Source Created Succesfully!");
        }

        [HttpDelete("{walletId}/{incomeSourceId}")]

        public async Task<IActionResult> DeleteIncomeSource(int walletId, int incomeSourceId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier!).Value);

            await _incomeSourcesService.DeleteIncomeSource(userId, walletId, incomeSourceId);

            return NoContent();
        }
    }
}
