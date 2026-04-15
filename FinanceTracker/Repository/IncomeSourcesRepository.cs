using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class IncomeSourcesRepository : IIncomeSourcesRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public IncomeSourcesRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task CreateIncomeSource(Incomesource incomeSource)
        {
            await _context.Incomesources.AddAsync(incomeSource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIncomeSource(Incomesource incomeSource)
        {
            _context.Incomesources.Update(incomeSource);

            await _context.SaveChangesAsync();
        }

        public async Task<Incomesource?> GetIncomeSourceById(int incomeSourceId)
        {
            return await _context.Incomesources
                  .FirstOrDefaultAsync(i => i.Incomesourceid == incomeSourceId && i.Isactive == true);

        }

        public async Task<ICollection<Incomesource>> GetIncomeSourcesByWalletId(int walletId)
        {
            return await _context.Incomesources
                .Where(i => i.Walletid == walletId && i.Isactive == true)

                .ToListAsync();

        }
    }
}
