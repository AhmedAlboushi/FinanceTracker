using FinanceTracker.Dtos.BugReportDto;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class BugReportsService : IBugReportsService
    {
        private readonly IBugReportsRepository _bugReportsRepository;

        public BugReportsService(IBugReportsRepository bugReportsRepository)
        {
            _bugReportsRepository = bugReportsRepository;
        }
        public async Task CreateBugReport(int userId, CreateBugReportDto bugReportDto)
        {

            var bugReport = new Bugreport()
            {
                Description = bugReportDto.Description,
                Userid = userId,

                // default not reviewed yet
                Status = false,

            };
            await _bugReportsRepository.CreateBugReport(bugReport);
        }
    }
}
