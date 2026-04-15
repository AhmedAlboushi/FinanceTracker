using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IBugReportsRepository
    {

        public Task CreateBugReport(Bugreport bugReport);
    }
}
