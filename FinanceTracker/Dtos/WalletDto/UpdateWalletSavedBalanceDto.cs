using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletDto
{
    public class UpdateWalletSavedBalanceDto
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Transfer Balance must be non-negative.")]

        public decimal TransferBalance { get; set; }

    }
}
