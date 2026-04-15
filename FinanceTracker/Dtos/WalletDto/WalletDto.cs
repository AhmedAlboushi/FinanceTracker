using FinanceTracker.Enums;

namespace FinanceTracker.Dtos.WalletDto
{
    public class WalletDto
    {

        public int WalletId { get; set; }

        public string WalletName { get; set; } = null!;

        public decimal SavedBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public WalletRoleType Role { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
