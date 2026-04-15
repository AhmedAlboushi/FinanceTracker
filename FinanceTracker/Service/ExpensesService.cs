using FinanceTracker.Dtos.ExpenseDto;
using FinanceTracker.Enums;
using FinanceTracker.Guards;
using FinanceTracker.Helpers;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class ExpensesService : IExpensesService
    {
        private readonly IExpensesRepository _expensesRepository;
        private readonly IWalletRoleGuard _walletRoleGuard;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IWalletsRepository _walletsRepository;
        public ExpensesService(IExpensesRepository expensesRepository, IWalletRoleGuard walletRoleGuard, ICategoriesRepository categoriesRepository, IWalletsRepository walletsRepository)
        {
            _walletRoleGuard = walletRoleGuard;
            _expensesRepository = expensesRepository;
            _categoriesRepository = categoriesRepository;
            _walletsRepository = walletsRepository;
        }
        public async Task CreateExpense(int userId, int walletId, CreateExpenseDto expenseDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var category = await _categoriesRepository.GetCategory(expenseDto.CategoryId);

            if (category == null)
                throw new KeyNotFoundException("category Doesn't Exist");

            if (category.Walletid != walletId)
                throw new KeyNotFoundException("category Doesn't Exist");

            var wallet = await _walletsRepository.GetWallet(walletId);

            if (wallet.Availablebalance < expenseDto.Amount)
                throw new InvalidOperationException("Not Enough Balance");

            wallet.Availablebalance -= expenseDto.Amount;

            await _walletsRepository.UpdateWallet(wallet);

            var expense = new Expense()
            {
                Amount = expenseDto.Amount,
                Createdat = DateOnly.FromDateTime(DateTime.UtcNow),
                Categoryid = expenseDto.CategoryId,
                Description = string.IsNullOrWhiteSpace(expenseDto.Description) ? null : expenseDto.Description,
                Walletid = walletId,
                Userid = userId,
            };

            await _expensesRepository.CreateExpense(expense);





        }

        public async Task<ICollection<ExpenseDto>> GetExpensesBetweenDates(int userId, int walletId, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);


            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);



            var expenses = await _expensesRepository.GetExpensesBetweenDates(walletId, startDate, endDate, page, pageSize);

            return expenses.Select(e => new ExpenseDto()
            {
                Amount = e.Amount,
                CreatedAt = e.Createdat,
                Category = e.Category.Categoryname,
                Description = e.Description,
                Username = e.User.Username,
                ExpenseId = e.Expenseid,
            }).ToList();
        }

        public async Task<ICollection<ExpenseDto>> GetExpensesByCategoryId(int userId, int walletId, int categoryId, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);


            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);



            var expenses = await _expensesRepository.GetExpensesByCategoryId(walletId, categoryId, page, pageSize);

            return expenses.Select(e => new ExpenseDto()
            {
                Amount = e.Amount,
                CreatedAt = e.Createdat,
                Category = e.Category.Categoryname,
                Description = e.Description,
                Username = e.User.Username,
                ExpenseId = e.Expenseid,
            }).ToList();
        }

        public async Task<ICollection<ExpenseDto>> GetExpensesByDate(int userId, int walletId, DateOnly date, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);


            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);



            var expenses = await _expensesRepository.GetExpensesByDate(walletId, date, page, pageSize);

            return expenses.Select(e => new ExpenseDto()
            {
                Amount = e.Amount,
                CreatedAt = e.Createdat,
                Category = e.Category.Categoryname,
                Description = e.Description,
                Username = e.User.Username,
                ExpenseId = e.Expenseid,
            }).ToList();
        }

        public async Task<ICollection<ExpenseDto>> GetExpensesByWalletId(int userId, int walletId, int page = 1, int pageSize = 20)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);


            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);



            var expenses = await _expensesRepository.GetExpensesByWalletId(walletId, page, pageSize);

            return expenses.Select(e => new ExpenseDto()
            {
                Amount = e.Amount,
                CreatedAt = e.Createdat,
                Category = e.Category.Categoryname,
                Description = e.Description,
                Username = e.User.Username,
                ExpenseId = e.Expenseid,
            }).ToList();
        }

        public async Task<decimal> GetTotalExpensesBetweenDates(int userid, int walletId, DateOnly startDate, DateOnly endDate)
        {
            await _walletRoleGuard.Authorize(userid, walletId, WalletRoleType.Viewer);

            return await _expensesRepository.GetTotalExpensesBetweenDates(walletId, startDate, endDate);
        }
    }
}
