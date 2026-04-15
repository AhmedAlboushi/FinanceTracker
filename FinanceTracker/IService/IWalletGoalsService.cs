using FinanceTracker.Dtos.WalletGoalDto;

namespace FinanceTracker.IService
{
    public interface IWalletGoalsService
    {
        public Task<ICollection<WalletGoalDto>> GetGoalsByWalletId(int userId, int walletId);


        public Task AllocateAmountToGoal(int userId, int walletId, int goalId, AllocateAmountToGoalDto walletGoalDto);
        public Task<int> CreateGoal(int userId, int walletId, CreateWalletGoalDto walletGoalDto);

        public Task AddImageToGoal(int userId, int walletId, int goalId, IFormFile goalImage);

        public Task DeleteGoal(int userId, int walletId, int goalId);

        public Task UpdateGoal(int userId, int walletId, int goalId, UpdateWalletGoalDto walletGoalDto);
    }
}
