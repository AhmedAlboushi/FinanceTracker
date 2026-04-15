using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public ExpensesRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }
        public async Task CreateExpense(Expense expense)
        {
            await _context.AddAsync(expense);

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Expense>> GetExpensesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                      .Include(e => e.User)

                .Where(e => e.Createdat >= startDate && e.Createdat <= endDate && e.Walletid == walletId)
                      .OrderByDescending(e => e.Createdat)

                         .Skip((page - 1) * pageSize)
                .Take(pageSize)
                 .ToListAsync();
        }

        public async Task<ICollection<Expense>> GetExpensesByCategoryId(int walletId, int categoryId, int page = 1, int pageSize = 20)
        {
            return await _context.Expenses
             .Include(e => e.Category)
                   .Include(e => e.User)

             .Where(e => e.Walletid == walletId && e.Categoryid == categoryId)
                   .OrderByDescending(e => e.Createdat)

                      .Skip((page - 1) * pageSize)
             .Take(pageSize)
              .ToListAsync();
        }

        public async Task<ICollection<Expense>> GetExpensesByDate(int walletId, DateOnly date, int page = 1, int pageSize = 20)
        {
            return await _context.Expenses
          .Include(e => e.Category)
                .Include(e => e.User)

          .Where(e => e.Walletid == walletId && e.Createdat == date)
                .OrderByDescending(e => e.Createdat)

                   .Skip((page - 1) * pageSize)
          .Take(pageSize)
           .ToListAsync();
        }

        public async Task<ICollection<Expense>> GetExpensesByWalletId(int walletId, int page = 1, int pageSize = 20)
        {
            return await _context.Expenses
      .Include(e => e.Category)
      .Include(e => e.User)
      .Where(e => e.Walletid == walletId)
            .OrderByDescending(e => e.Createdat)

               .Skip((page - 1) * pageSize)
      .Take(pageSize)
       .ToListAsync();
        }

        public async Task<decimal> GetTotalExpensesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate)
        {
            return await _context.Expenses
                 .Where(i => i.Walletid == walletId && i.Createdat >= startDate && i.Createdat <= endDate)
                 .SumAsync(i => i.Amount);
        }
    }
}
