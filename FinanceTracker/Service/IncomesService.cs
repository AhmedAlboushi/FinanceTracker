using FinanceTracker.Dtos.IncomeDto;
using FinanceTracker.Enums;
using FinanceTracker.Guards;
using FinanceTracker.Helpers;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class IncomesService : IIncomesService
    {
        private readonly IIncomesRepository _incomesRepository;
        private readonly IWalletRoleGuard _walletRoleGuard;
        private readonly IIncomeSourcesRepository _incomeSourcesRepository;
        private readonly IWalletsRepository _walletsRepository;

        public IncomesService(IIncomesRepository incomesRepository, IWalletRoleGuard walletRoleGuard,
            IIncomeSourcesRepository incomeSourcesRepository, IWalletsRepository walletsRepository)
        {
            _incomesRepository = incomesRepository;
            _walletRoleGuard = walletRoleGuard;
            _incomeSourcesRepository = incomeSourcesRepository;
            _walletsRepository = walletsRepository;
        }

        public async Task CreateIncome(int userId, int walletId, CreateIncomeDto incomeDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var incomeSource = await _incomeSourcesRepository.GetIncomeSourceById(incomeDto.IncomeSourceId);

            if (incomeSource == null)
                throw new KeyNotFoundException("Source Doesn't Exist");



            if (incomeSource.Walletid != walletId)
                throw new UnauthorizedAccessException("Source isn't connected to wallet");

            var income = new Income()
            {
                Amount = incomeDto.Amount,
                Createdat = DateOnly.FromDateTime(DateTime.UtcNow),
                Description = string.IsNullOrWhiteSpace(incomeDto.Description) ? null : incomeDto.Description,
                Userid = userId,
                Walletid = walletId,
                Incomesourceid = incomeDto.IncomeSourceId,
            };
            await _incomesRepository.CreateIncome(income);


            var wallet = await _walletsRepository.GetWallet(walletId);


            wallet.Availablebalance += incomeDto.Amount;

            await _walletsRepository.UpdateWallet(wallet);
        }

        public async Task<ICollection<IncomeDto>> GetIncomesBetweenDates(int userId, int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);


            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);


            var incomes = await _incomesRepository.GetIncomesBetweenDates(walletId, startDate, endDate, page, pageSize);

            return incomes.Select(i => new IncomeDto()
            {
                Amount = i.Amount,
                CreatedAt = i.Createdat,
                Description = i.Description,
                IncomeId = i.Incomeid,
                IncomeSource = i.Incomesource.Incomesourcename,
                Username = i.User.Username,

            }).ToList();

        }

        public async Task<ICollection<IncomeDto>> GetIncomesByDate(int userId, int walletId, DateOnly date, int page = 1, int pageSize = 20)
        {

            (page, pageSize) = PaginationHelper.Validate(page, pageSize);

            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);


            var incomes = await _incomesRepository.GetIncomesByDate(walletId, date, page, pageSize);

            return incomes.Select(i => new IncomeDto()
            {
                Amount = i.Amount,
                CreatedAt = i.Createdat,
                Description = i.Description,
                IncomeId = i.Incomeid,
                IncomeSource = i.Incomesource.Incomesourcename,
                Username = i.User.Username,

            }).ToList();
        }

        public async Task<ICollection<IncomeDto>> GetIncomesByIncomeSourceId(int userId, int walletId, int incomeSourceId, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);


            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            if (incomeSourceId < 0)
                throw new InvalidOperationException();

            var incomes = await _incomesRepository.GetIncomesByIncomeSourceId(walletId, incomeSourceId, page, pageSize);

            return incomes.Select(i => new IncomeDto()
            {
                Amount = i.Amount,
                CreatedAt = i.Createdat,
                Description = i.Description,
                IncomeId = i.Incomeid,
                IncomeSource = i.Incomesource.Incomesourcename,
                Username = i.User.Username,

            }).ToList();
        }

        public async Task<ICollection<IncomeDto>> GetIncomesByWalletId(int userId, int walletId, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);

            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            var incomes = await _incomesRepository.GetIncomesByWalletId(walletId, page, pageSize);


            return incomes.Select(i => new IncomeDto()
            {
                Amount = i.Amount,
                CreatedAt = i.Createdat,
                Description = i.Description,
                IncomeId = i.Incomeid,
                IncomeSource = i.Incomesource.Incomesourcename,
                Username = i.User.Username,

            }).ToList();
        }

        public async Task<decimal> GetTotalIncomeBetweenDates(int userId, int walletId, DateOnly startDate, DateOnly endDate)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            return await _incomesRepository.GetTotalIncomeBetweenDates(walletId, startDate, endDate);
        }
    }
}
