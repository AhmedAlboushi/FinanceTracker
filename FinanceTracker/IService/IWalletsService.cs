using FinanceTracker.Dtos.WalletDto;
using FinanceTracker.Enums;

namespace FinanceTracker.IService
{
    public interface IWalletsService
    {

        Task<WalletDto> GetWallet(int userId, int walletId);
        Task<ICollection<WalletDto>> GetWalletsByUserId(int userId);

        Task<ICollection<UsersConnectedToWalletDto>> GetUsersConnectedToWallet(int userId, int walletId);

        Task AddUserToWallet(int userId, int walletId, int targetUserId, WalletRoleType role = WalletRoleType.Viewer);
        Task<WalletDto> GetWalletByOwnerRole(int userId);

        Task CreateWallet(int userId, CreateWalletDto walletDto);
        Task UpdateWalletName(int userId, int walletId, UpdateWalletNameDto walletDto);
        Task SaveBalance(int userId, int walletId, UpdateWalletSavedBalanceDto walletDto);

        Task RemoveUserFromWallet(int userId, int targetUserId, int walletId);

        Task LeaveWallet(int userId, int walletId);

        public Task TransferSavedBalanceToAvailableBalance(int userId, int walletId, TransferSavedBalanceToAvailableBalanceDto walletDto);

    }
}
