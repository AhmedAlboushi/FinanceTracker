using FinanceTracker.Enums;
using FinanceTracker.IRepository;

namespace FinanceTracker.Guards
{
    public class WalletRoleGuard : IWalletRoleGuard
    {
        private readonly IUserWalletsRepository _userWalletsRepository;

        public WalletRoleGuard(IUserWalletsRepository userWalletsRepository)
        {
            _userWalletsRepository = userWalletsRepository;
        }

        public async Task Authorize(int userId, int walletId, WalletRoleType minimumRole)
        {
            var userWallet = await _userWalletsRepository.GetUserWalletByUserIdAndWalletId(userId, walletId);
            if (userWallet == null)
                throw new UnauthorizedAccessException();

            var role = (WalletRoleType)userWallet.Walletroleid;

            if (role > minimumRole) // Owner=1, Editor=2, Viewer=3
                throw new UnauthorizedAccessException();

        }
    }
}
