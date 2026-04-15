using FinanceTracker.Dtos.ExpenseDto;
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

    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }


        [HttpGet("{walletId}/total-by-date-range")]

        public async Task<IActionResult> GetTotalExpensesBetweenDates(int walletId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)

        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var totalExpense = await _expensesService.GetTotalExpensesBetweenDates(userId, walletId, startDate, endDate);

            return Ok(totalExpense);
        }
        [HttpPost("{walletId}")]

        public async Task<IActionResult> CreateExpense(int walletId, CreateExpenseDto expenseDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _expensesService.CreateExpense(userId, walletId, expenseDto);

            return Ok("Created Expense Succesfully!");
        }

        [HttpGet("{walletId}/between-dates")]
        public async Task<IActionResult> GetExpensesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20)
        {
            if (startDate == default || endDate == default)
                return BadRequest("Date is required");
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expensesDto = await _expensesService.GetExpensesBetweenDates(userId, walletId, startDate, endDate, page = 1, pageSize = 20);

            return Ok(expensesDto);
        }

        [HttpGet("{walletId}/by-walletId")]
        public async Task<IActionResult> GetExpensesByWalletId(int walletId, int page = 1, int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expensesDto = await _expensesService.GetExpensesByWalletId(userId, walletId, page, pageSize);

            return Ok(expensesDto);
        }

        [HttpGet("{walletId}/by-categoryId/{categoryId}")]
        public async Task<IActionResult> GetExpensesByCategoryId(int walletId, int categoryId, int page = 1, int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expensesDto = await _expensesService.GetExpensesByCategoryId(userId, walletId, categoryId, page, pageSize);

            return Ok(expensesDto);
        }


        [HttpGet("{walletId}/by-date")]
        public async Task<IActionResult> GetExpensesByDate(int walletId, DateOnly date, int page = 1, int pageSize = 20)
        {
            if (date == default)
                return BadRequest("Date is required");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expensesDto = await _expensesService.GetExpensesByDate(userId, walletId, date, page, pageSize);

            return Ok(expensesDto);
        }
    }
}
