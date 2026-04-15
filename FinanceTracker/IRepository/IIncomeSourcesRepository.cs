using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IIncomeSourcesRepository
    {
        public Task<ICollection<Incomesource>> GetIncomeSourcesByWalletId(int walletId);

        public Task<Incomesource?> GetIncomeSourceById(int incomeSourceId);

        public Task CreateIncomeSource(Incomesource incomeSource);

        public Task DeleteIncomeSource(Incomesource incomeSource);

    }
}
