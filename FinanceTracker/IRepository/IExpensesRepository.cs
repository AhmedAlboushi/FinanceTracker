using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IExpensesRepository
    {
        public Task<ICollection<Expense>> GetExpensesByWalletId(int walletId, int page = 1, int pageSize = 20);

        public Task<ICollection<Expense>> GetExpensesByCategoryId(int walletId, int categoryId, int page = 1, int pageSize = 20);

        public Task<ICollection<Expense>> GetExpensesByDate(int walletId, DateOnly date, int page = 1, int pageSize = 20);

        public Task<ICollection<Expense>> GetExpensesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20);

        public Task<decimal> GetTotalExpensesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate);
        public Task CreateExpense(Expense expense);
    }
}
