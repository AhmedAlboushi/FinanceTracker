using FinanceTracker.Dtos.BugReportDto;

namespace FinanceTracker.IService
{
    public interface IBugReportsService
    {

        public Task CreateBugReport(int userId, CreateBugReportDto bugReportDto);
    }
}
