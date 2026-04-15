using FinanceTracker.Models;

namespace FinanceTracker.IRepository
{
    public interface IWalletGoalsRepository
    {
        public Task<ICollection<Walletgoal>> GetGoalsByWalletId(int walletId);


        public Task<Walletgoal?> GetGoalByWalletIdAndGoalId(int walletId, int goalId);
        public Task CreateGoal(Walletgoal walletGoal);

        public Task DeleteGoal(int goalId);

        public Task UpdateGoal(Walletgoal walletGoal);

    }
}
