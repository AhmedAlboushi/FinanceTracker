using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.WalletDto
{
    public class CreateWalletDto
    {

        [Required]
        [StringLength(50)]

        public string WalletName { get; set; } = null!;

    }
}
