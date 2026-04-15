using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletGoalDto
{
    public class CreateWalletGoalDto
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
        public byte Priority { get; set; }



    }
}
