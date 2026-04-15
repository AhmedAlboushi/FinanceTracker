using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletDto
{
    public class TransferSavedBalanceToAvailableBalanceDto
    {
        [Required]
        public int TransferBalance { get; set; }
    }
}
