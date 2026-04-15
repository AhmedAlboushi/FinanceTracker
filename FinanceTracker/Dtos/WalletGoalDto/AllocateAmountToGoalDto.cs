using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletGoalDto
{
    public class AllocateAmountToGoalDto
    {



        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]

        public decimal AllocatedAmount { get; set; }


    }
}
