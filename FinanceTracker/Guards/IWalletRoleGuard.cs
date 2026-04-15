using FinanceTracker.Enums;

namespace FinanceTracker.Guards
{
    public interface IWalletRoleGuard
    {
        Task Authorize(int userId, int walletId, WalletRoleType minimumRole);

    }
}
