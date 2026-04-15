using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface ICategoriesRepository
    {

        public Task<ICollection<Category>> GetCategoriesByWalletId(int walleTId);

        public Task<Category> GetCategory(int categoryId);

        public Task CreateCategory(Category category);

        public Task DeleteCategory(Category categoryId);
    }
}
