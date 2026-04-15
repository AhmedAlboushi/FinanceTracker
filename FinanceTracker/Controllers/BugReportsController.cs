using FinanceTracker.Dtos.BugReportDto;
using FinanceTracker.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AuthLimiter")]
    [Authorize]
    public class BugReportsController : ControllerBase
    {

        private readonly IBugReportsService _bugReportsService;

        public BugReportsController(IBugReportsService bugReportsService)
        {
            _bugReportsService = bugReportsService;
        }


        [HttpPost]

        public async Task<IActionResult> ReportABug(CreateBugReportDto bugReportDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _bugReportsService.CreateBugReport(userId, bugReportDto);
            return StatusCode(201);
        }


    }
}
