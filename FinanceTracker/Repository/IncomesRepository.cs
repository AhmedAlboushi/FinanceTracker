using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class IncomesRepository : IIncomesRepository
    {
        private readonly FinanceTrackerDbContext _financeTrackerDbContext;

        public IncomesRepository(FinanceTrackerDbContext financeTrackerDbContext)
        {
            _financeTrackerDbContext = financeTrackerDbContext;
        }
        public async Task CreateIncome(Income income)
        {
            await _financeTrackerDbContext.Incomes.AddAsync(income);
            await _financeTrackerDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Income>> GetIncomesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate,
            int page = 1, int pageSize = 20)
        {
            return await _financeTrackerDbContext.Incomes
                  .Include(i => i.Incomesource)
                    .Include(i => i.User)
                  .Where(i => i.Walletid == walletId && i.Createdat >= startDate && i.Createdat <= endDate)
                                                        .OrderByDescending(i => i.Createdat)

                      .Skip((page - 1) * pageSize)
                  .Take(pageSize)

                  .ToListAsync();
        }

        public async Task<ICollection<Income>> GetIncomesByDate(int walletId, DateOnly date, int page = 1, int pageSize = 20)
        {
            return await _financeTrackerDbContext.Incomes
                                .Include(i => i.Incomesource)
                                  .Include(i => i.User)
                .Where(i => i.Walletid == walletId && i.Createdat == date)
                                      .OrderByDescending(i => i.Createdat)

                .Skip((page - 1) * pageSize)
                .Take(pageSize)

                .ToListAsync();
        }

        public async Task<ICollection<Income>> GetIncomesByIncomeSourceId(int walletId, int incomeSourceId,
            int page = 1, int pageSize = 20)
        {
            return await _financeTrackerDbContext.Incomes
                                .Include(i => i.Incomesource)
                                  .Include(i => i.User)
                .Where(i => i.Walletid == walletId && i.Incomesourceid == incomeSourceId)
                                      .OrderByDescending(i => i.Createdat)

                  .Skip((page - 1) * pageSize)
                .Take(pageSize)

                .ToListAsync();
        }

        public async Task<ICollection<Income>> GetIncomesByWalletId(int walletId, int page = 1, int pageSize = 20)
        {
            return await _financeTrackerDbContext.Incomes
                                .Include(i => i.Incomesource)
                                .Include(i => i.User)
                .Where(i => i.Walletid == walletId)
                                      .OrderByDescending(i => i.Createdat)

                   .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalIncomeBetweenDates(int walletId, DateOnly startDate, DateOnly endDate)
        {
            return await _financeTrackerDbContext.Incomes
                 .Where(i => i.Walletid == walletId && i.Createdat >= startDate && i.Createdat <= endDate)
                 .SumAsync(i => i.Amount);
        }
    }
}
