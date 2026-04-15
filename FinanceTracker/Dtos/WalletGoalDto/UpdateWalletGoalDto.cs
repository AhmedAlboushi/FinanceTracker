using FinanceTracker.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletGoalDto
{
    public class UpdateWalletGoalDto
    {


        [Required]
        [StringLength(50)]

        public string GoalName { get; set; } = null!;

        [StringLength(500)]

        public string? Description { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "TargetAmount must be non-negative.")]

        public decimal TargetAmount { get; set; }

        public DateOnly? TargetDate { get; set; }

        [Required]

        [Range(1, 3, ErrorMessage = "Priority must be 1 (High), 2 (Medium), or 3 (Low)")]
        public WalletGoalPriority Priority { get; set; }

    }
}
