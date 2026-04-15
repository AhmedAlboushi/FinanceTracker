using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletDto
{
    public class UpdateWalletNameDto
    {
        [Required]

        public string WalletName { get; set; } = null!;



    }
}
