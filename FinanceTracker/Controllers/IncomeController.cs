using FinanceTracker.Dtos.IncomeDto;
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

    public class IncomesController : ControllerBase
    {
        private readonly IIncomesService _incomesService;
        public IncomesController(IIncomesService incomesService)
        {
            _incomesService = incomesService;
        }

        [HttpGet("{walletId}/total-by-date-range")]

        public async Task<IActionResult> GetTotalIncomeBetweenDates(int walletId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)

        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var totalIncome = await _incomesService.GetTotalIncomeBetweenDates(userId, walletId, startDate, endDate);

            return Ok(totalIncome);
        }


        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetIncomesByWalletId(int walletId, int page = 1, int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var incomesDto = await _incomesService.GetIncomesByWalletId(userId, walletId, page, pageSize);

            return Ok(incomesDto);
        }
        [HttpGet("{walletId}/by-source/{incomeSourceId}")]

        public async Task<IActionResult> GetIncomesByIncomeSourceId(int walletId,
            int incomeSourceId, int page = 1, int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var incomesDto = await _incomesService.GetIncomesByIncomeSourceId(userId, walletId, incomeSourceId, page);

            return Ok(incomesDto);
        }

        [HttpGet("{walletId}/by-date")]

        public async Task<IActionResult> GetIncomesByDate(int walletId, DateOnly date, int page = 1, int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var incomesDto = await _incomesService.GetIncomesByDate(userId, walletId, date, page);

            return Ok(incomesDto);
        }

        [HttpGet("{walletId}/between-dates")]
        public async Task<IActionResult> GetIncomesBetweenDates(int walletId, DateOnly startDate,
            DateOnly endDate, int page = 1, int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var incomesDto = await _incomesService.GetIncomesBetweenDates(userId, walletId, startDate, endDate, page, pageSize);

            return Ok(incomesDto);

        }
        [HttpPost("{walletId}")]
        public async Task<IActionResult> CreateIncome(int walletId, CreateIncomeDto incomeDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _incomesService.CreateIncome(userId, walletId, incomeDto);

            return Ok($"Added {incomeDto.Amount} To wallet Succesfully!");
        }


    }
}
