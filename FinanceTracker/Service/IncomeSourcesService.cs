using FinanceTracker.Dtos.IncomeSourceDto;
using FinanceTracker.Enums;
using FinanceTracker.Guards;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class IncomeSourcesService : IIncomeSourcesService
    {
        private readonly IIncomeSourcesRepository _incomeSourcesRepository;
        private readonly IWalletRoleGuard _walletRoleGuard;
        public IncomeSourcesService(IIncomeSourcesRepository incomeSourcesRepository, IWalletRoleGuard walletRoleGuard)
        {
            _walletRoleGuard = walletRoleGuard;
            _incomeSourcesRepository = incomeSourcesRepository;
        }

        public async Task CreateIncomeSource(int userId, int walletId, CreateIncomeSourceDto incomeSourceDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var incomeSource = new Incomesource()
            {
                Isactive = true,
                Incomesourcename = incomeSourceDto.IncomeSourceName,
                Walletid = walletId,

            };
            await _incomeSourcesRepository.CreateIncomeSource(incomeSource);
        }

        public async Task DeleteIncomeSource(int userId, int walletId, int incomeSourceId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);


            var incomeSource = await _incomeSourcesRepository.GetIncomeSourceById(incomeSourceId);

            if (incomeSource == null)
                throw new KeyNotFoundException("Income source doesn't exist");

            incomeSource.Isactive = false;

            await _incomeSourcesRepository.DeleteIncomeSource(incomeSource);
        }

        public async Task<IncomeSourceDto?> GetIncomeSourceById(int userId, int walletId, int incomeSourceId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            var incomeSource = await _incomeSourcesRepository.GetIncomeSourceById(incomeSourceId);

            if (incomeSource == null)
                throw new KeyNotFoundException("IncomeSource Doesn't Exist");

            return new IncomeSourceDto()
            {
                IncomeSourceId = incomeSourceId,
                IncomeSourceName = incomeSource.Incomesourcename
            };
        }

        public async Task<ICollection<IncomeSourceDto>> GetIncomeSourcesByWalletId(int userId, int walletId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);
            var incomeSources = await _incomeSourcesRepository.GetIncomeSourcesByWalletId(walletId);

            if (incomeSources == null)
                throw new KeyNotFoundException("No Income Sources Found!");

            return incomeSources.Select(i => new IncomeSourceDto()
            {
                IncomeSourceId = i.Incomesourceid,
                IncomeSourceName = i.Incomesourcename
            }).ToList();
        }
    }
}
