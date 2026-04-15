using FinanceTracker.Dtos.ExpenseDto;

namespace FinanceTracker.IService
{
    public interface IExpensesService
    {
        public Task<ICollection<ExpenseDto>> GetExpensesByWalletId(int userId, int walletId, int page = 1, int pageSize = 20);

        public Task<ICollection<ExpenseDto>> GetExpensesByCategoryId(int userId, int walletId, int categoryId, int page = 1, int pageSize = 20);

        public Task<ICollection<ExpenseDto>> GetExpensesByDate(int userId, int walletId, DateOnly date, int page = 1, int pageSize = 20);

        public Task<ICollection<ExpenseDto>> GetExpensesBetweenDates(int userId, int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20);
        public Task<decimal> GetTotalExpensesBetweenDates(int userid, int walletId, DateOnly startDate, DateOnly endDate);

        public Task CreateExpense(int userId, int walletId, CreateExpenseDto expenseDto);
    }
}
