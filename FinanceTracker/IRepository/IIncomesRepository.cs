using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IIncomesRepository
    {
        public Task<ICollection<Income>> GetIncomesByWalletId(int walletId, int page = 1, int pageSize = 20);

        public Task<ICollection<Income>> GetIncomesByIncomeSourceId(int walletId, int incomeSourceId, int page = 1, int pageSize = 20);
        public Task<ICollection<Income>> GetIncomesByDate(int walletId, DateOnly date, int page = 1, int pageSize = 20);

        public Task<ICollection<Income>> GetIncomesBetweenDates(int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20);

        public Task<decimal> GetTotalIncomeBetweenDates(int walletId, DateOnly startDate, DateOnly endDate);
        public Task CreateIncome(Income income);

    }
}
