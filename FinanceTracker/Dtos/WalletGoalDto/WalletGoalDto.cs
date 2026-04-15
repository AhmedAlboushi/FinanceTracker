using FinanceTracker.Enums;

namespace FinanceTracker.Dtos.WalletGoalDto
{
    public class WalletGoalDto
    {
        public int WalletGoalId { get; set; }

        public string GoalName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal AllocatedAmount { get; set; }

        public decimal TargetAmount { get; set; }

        public DateOnly? TargetDate { get; set; }

        public bool IsCompleted { get; set; }

        public WalletGoalPriority Priority { get; set; }

        public string? GoalImageUrl { get; set; }

        public DateOnly CreatedAt { get; set; }

    }
}
