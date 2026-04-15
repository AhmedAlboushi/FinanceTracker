using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly FinanceTrackerDbContext _context;

        public CategoriesRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task CreateCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Category>> GetCategoriesByWalletId(int walletId)
        {
            return await _context.Categories
                 .Where(c => c.Walletid == walletId && c.Isactive == true)
                 .ToListAsync();
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Categoryid == categoryId && c.Isactive == true);
        }
    }
}
