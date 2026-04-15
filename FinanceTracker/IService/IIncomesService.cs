using FinanceTracker.Dtos.IncomeDto;

namespace FinanceTracker.IService
{
    public interface IIncomesService
    {
        public Task<ICollection<IncomeDto>> GetIncomesByWalletId(int userId, int walletId, int page = 1, int pageSize = 20);

        public Task<ICollection<IncomeDto>> GetIncomesByIncomeSourceId(int userId, int walletId, int incomeSourceId, int page = 1, int pageSize = 20);
        public Task<ICollection<IncomeDto>> GetIncomesByDate(int userId, int walletId, DateOnly date, int page = 1, int pageSize = 20);

        public Task<ICollection<IncomeDto>> GetIncomesBetweenDates(int userId, int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20);
        public Task<decimal> GetTotalIncomeBetweenDates(int userId, int walletId, DateOnly startDate, DateOnly endDate);


        public Task CreateIncome(int userId, int walletId, CreateIncomeDto incomeDto);
    }
}
