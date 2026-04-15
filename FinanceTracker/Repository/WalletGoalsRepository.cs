using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class WalletGoalsRepository : IWalletGoalsRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public WalletGoalsRepository(FinanceTrackerDbContext context)

        {
            _context = context;
        }
        public async Task CreateGoal(Walletgoal walletGoal)
        {
            await _context.AddAsync(walletGoal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGoal(int goalId)
        {
            await _context.Walletgoals
                .Where(g => g.Walletgoalid == goalId)
                .ExecuteDeleteAsync();
        }

        public async Task<Walletgoal?> GetGoalByWalletIdAndGoalId(int walletId, int goalId)
        {
            return await _context.Walletgoals
                .Where(w => w.Walletid == walletId && w.Walletgoalid == goalId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Walletgoal>> GetGoalsByWalletId(int walletId)
        {
            return await _context.Walletgoals
                .Where(w => w.Walletid == walletId)
                .ToListAsync();
        }

        public async Task UpdateGoal(Walletgoal walletGoal)
        {
            _context.Walletgoals.Update(walletGoal);
            await _context.SaveChangesAsync(true);
        }
    }
}
