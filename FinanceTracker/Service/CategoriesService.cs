using FinanceTracker.Dtos.CategoryDto;
using FinanceTracker.Enums;
using FinanceTracker.Guards;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IWalletRoleGuard _walletRoleGuard;
        public CategoriesService(ICategoriesRepository categoriesRepository, IWalletRoleGuard walletRoleGuard)

        {
            _walletRoleGuard = walletRoleGuard;
            _categoriesRepository = categoriesRepository;
        }

        public async Task CreateCategory(int userId, int walletId, CreateCategoryDto createCategoryDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var category = new Category()
            {
                Isactive = true,
                Categoryname = createCategoryDto.CategoryName,
                // ColorHex = string.IsNullOrWhiteSpace(createCategoryDto.ColorHex) ? "#000000" : createCategoryDto.ColorHex,
                //IconName = string.IsNullOrWhiteSpace(createCategoryDto.IconName) ? null : createCategoryDto.IconName,
                Walletid = walletId,

            };
            await _categoriesRepository.CreateCategory(category);
        }

        public async Task DeleteCategory(int userId, int walletId, int categoryId)
        {



            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var category = await _categoriesRepository.GetCategory(categoryId);

            if (category == null)
                throw new KeyNotFoundException("Category Doesn't exist");

            category.Isactive = false;

            await _categoriesRepository.DeleteCategory(category);
        }

        public async Task<ICollection<CategoryDto>> GetCategoriesByWalletId(int userId, int walleTId)
        {
            await _walletRoleGuard.Authorize(userId, walleTId, WalletRoleType.Viewer);

            var categories = await _categoriesRepository.GetCategoriesByWalletId(walleTId);

            return categories.Select(c => new CategoryDto()
            {
                CategoryId = c.Categoryid,
                CategoryName = c.Categoryname,
                ColorHex = c.Colorhex,
                IconName = c?.Iconname,
            }).ToList();
        }


    }
}
