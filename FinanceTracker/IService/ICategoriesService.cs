using FinanceTracker.Dtos.CategoryDto;

namespace FinanceTracker.IService
{
    public interface ICategoriesService
    {
        public Task<ICollection<CategoryDto>> GetCategoriesByWalletId(int userId, int walletId);
        public Task CreateCategory(int userId, int walletId, CreateCategoryDto createCategoryDto);

        public Task DeleteCategory(int userId, int walletId, int categoryId);
    }
}
