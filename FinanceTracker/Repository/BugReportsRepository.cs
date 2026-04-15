using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;

namespace FinanceTracker.Repository
{
    public class BugReportsRepository : IBugReportsRepository
    {

        private readonly FinanceTrackerDbContext _context;
        public BugReportsRepository(FinanceTrackerDbContext context)
        {
            _context = context;

        }
        public async Task CreateBugReport(Bugreport bugReport)
        {
            await _context.Bugreports.AddAsync(bugReport);

            await _context.SaveChangesAsync();
        }
    }
}
