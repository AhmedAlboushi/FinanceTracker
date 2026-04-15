using FinanceTracker.Dtos.WalletGoalDto;
using FinanceTracker.Enums;
using FinanceTracker.Guards;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class WalletGoalsService : IWalletGoalsService
    {
        private readonly IWalletGoalsRepository _walletGoalsRepository;
        private readonly IWalletRoleGuard _walletRoleGuard;
        private readonly IWalletsRepository _walletsRepository;
        private readonly IImageService _imageService;
        public WalletGoalsService(IWalletGoalsRepository walletGoalsRepository,
            IWalletRoleGuard walletRoleGuard,
            IWalletsRepository walletsRepository,
            IImageService imageService)
        {
            _walletRoleGuard = walletRoleGuard;
            _walletGoalsRepository = walletGoalsRepository;
            _walletsRepository = walletsRepository;
            _imageService = imageService;
        }

        public async Task AddImageToGoal(int userId, int walletId, int goalId, IFormFile goalImage)
        {

            if (goalImage == null)
                return;

            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var goal = await _walletGoalsRepository.GetGoalByWalletIdAndGoalId(walletId, goalId);


            if (goal.Goalimageurl != null)
            {
                await _imageService.DeleteImage(goal.Goalimageurl);
            }


            var publicId = await _imageService.UploadImage(goalImage);
            goal.Goalimageurl = publicId;

            await _walletGoalsRepository.UpdateGoal(goal);
        }

        public async Task AllocateAmountToGoal(int userId, int walletId, int goalId, AllocateAmountToGoalDto walletGoalDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var wallet = await _walletsRepository.GetWallet(walletId);

            if (wallet.Availablebalance < walletGoalDto.AllocatedAmount)
                throw new InvalidOperationException("Not Enough Balance To Preform This Operation");

            var walletGoal = await _walletGoalsRepository.GetGoalByWalletIdAndGoalId(walletId, goalId);

            if (walletGoal == null)
                throw new KeyNotFoundException("Goal Doesn't Exist");

            if (walletGoal.Iscompleted == true)
                throw new InvalidOperationException("Goal Already Completed");

            var oldAllocatedAmount = walletGoal.Allocatedamount;

            walletGoal.Allocatedamount += walletGoalDto.AllocatedAmount;
            wallet.Availablebalance -= walletGoalDto.AllocatedAmount;


            // if user passed more than the targetamount needed we Complete the Goal and return the extra to the wallet
            if (walletGoal.Targetamount <= walletGoal.Allocatedamount)
            {
                walletGoal.Allocatedamount = walletGoal.Targetamount;
                walletGoal.Iscompleted = true;
                var extraBalance = walletGoalDto.AllocatedAmount - (walletGoal.Targetamount - oldAllocatedAmount);

                wallet.Availablebalance += extraBalance;

            }
            await _walletsRepository.UpdateWallet(wallet);

            await _walletGoalsRepository.UpdateGoal(walletGoal);

        }

        public async Task<int> CreateGoal(int userId, int walletId, CreateWalletGoalDto walletGoalDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);


            var walletGoal = new Walletgoal()
            {
                Allocatedamount = 0,
                Createdat = DateOnly.FromDateTime(DateTime.UtcNow),
                Targetamount = walletGoalDto.TargetAmount,
                Description = string.IsNullOrWhiteSpace(walletGoalDto.Description) ? null : walletGoalDto.Description,
                Goalname = walletGoalDto.GoalName,
                Iscompleted = false,
                Priority = walletGoalDto.Priority,
                Targetdate = walletGoalDto.TargetDate,
                Walletid = walletId,
            };
            await _walletGoalsRepository.CreateGoal(walletGoal);
            return walletGoal.Walletgoalid;
        }

        public async Task DeleteGoal(int userId, int walletId, int goalId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var goal = await _walletGoalsRepository.GetGoalByWalletIdAndGoalId(walletId, goalId);

            if (goal == null)
                throw new KeyNotFoundException("Goal not found!");

            if (goal.Iscompleted == true)
                throw new InvalidOperationException("Cannot delete a completed goal!");

            if (goal.Allocatedamount > 0)
            {
                var wallet = await _walletsRepository.GetWallet(walletId);

                wallet.Availablebalance += goal.Allocatedamount;
                await _walletsRepository.UpdateWallet(wallet);
            }
            await _walletGoalsRepository.DeleteGoal(goalId);

        }

        public async Task<ICollection<WalletGoalDto>> GetGoalsByWalletId(int userId, int walletId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            var goals = await _walletGoalsRepository.GetGoalsByWalletId(walletId);

            var goalDtos = await Task.WhenAll(goals.Select(async g => new WalletGoalDto()
            {
                AllocatedAmount = g.Allocatedamount,
                CreatedAt = g.Createdat,
                TargetAmount = g.Targetamount,
                Description = g.Description,
                GoalImageUrl = g.Goalimageurl != null ? await _imageService.GenerateSignedUrl(g.Goalimageurl) : null,
                GoalName = g.Goalname,
                IsCompleted = g.Iscompleted,
                Priority = (WalletGoalPriority)g.Priority,
                TargetDate = g.Targetdate,
                WalletGoalId = g.Walletgoalid,
            }));

            return goalDtos.ToList();
        }

        public async Task UpdateGoal(int userId, int walletId, int goalId, UpdateWalletGoalDto walletGoalDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);

            var goal = await _walletGoalsRepository.GetGoalByWalletIdAndGoalId(walletId, goalId);

            if (goal == null)
                throw new KeyNotFoundException("Goal not found!");

            if (goal.Iscompleted == true)
                throw new InvalidOperationException("A completed goal cannot be editted!");

            goal.Description = walletGoalDto.Description;
            if (walletGoalDto.TargetAmount <= goal.Allocatedamount)
            {
                goal.Targetamount = goal.Allocatedamount;
                goal.Iscompleted = true;


            }
            else
                goal.Targetamount = walletGoalDto.TargetAmount;

            goal.Targetdate = walletGoalDto.TargetDate;
            goal.Goalname = walletGoalDto.GoalName;
            goal.Priority = (byte)walletGoalDto.Priority;

            await _walletGoalsRepository.UpdateGoal(goal);

        }
    }
}
