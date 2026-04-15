using FinanceTracker.Dtos.IncomeSourceDto;

namespace FinanceTracker.IService
{
    public interface IIncomeSourcesService
    {
        public Task<ICollection<IncomeSourceDto>> GetIncomeSourcesByWalletId(int userId, int walletId);

        public Task<IncomeSourceDto?> GetIncomeSourceById(int userId, int walletId, int incomeSourceId);

        public Task CreateIncomeSource(int userId, int walletId, CreateIncomeSourceDto incomeSourceDto);

        public Task DeleteIncomeSource(int userId, int walletId, int incomeSourceId);
    }
}
