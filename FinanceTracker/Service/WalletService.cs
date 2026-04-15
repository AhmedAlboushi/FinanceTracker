using FinanceTracker.Dtos.WalletDto;
using FinanceTracker.Enums;
using FinanceTracker.Guards;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class WalletService : IWalletsService
    {
        private readonly IWalletsRepository _walletsRepository;
        private readonly IUserWalletsRepository _userWalletsRepository;
        private readonly IWalletRoleGuard _walletRoleGuard;
        private readonly IUsersRepository _usersRepository;
        public WalletService(IWalletsRepository walletsRepository, IUserWalletsRepository userWalletsRepository
            , IWalletRoleGuard walletRoleGuard, IUsersRepository usersRepository)
        {
            _walletRoleGuard = walletRoleGuard;
            _walletsRepository = walletsRepository;
            _userWalletsRepository = userWalletsRepository;
            _usersRepository = usersRepository;
        }

  
        public async Task CreateWallet(int userId, CreateWalletDto walletDto)
        {
            var wallet = new Wallet()
            {
                Walletname = walletDto.WalletName

            };
            await _walletsRepository.CreateWallet(wallet);

            var userWallet = new Userwallet()
            {
                Walletid = wallet.Walletid,
                Userid = userId,
                Walletroleid = (short)WalletRoleType.Owner,
            };
            await _userWalletsRepository.CreateUserWallet(userWallet);



        }

        //public async Task Deposit(int userId,int walletId, UpdateWalletAvailableBalanceDto dto)
        //{
        //    await _walletRoleGuard.Authorize(userId,walletId,WalletRoleType.Editor);

        //    var wallet = await _walletsRepository.GetWallet(walletId);
        //    if (wallet == null)
        //        throw new InvalidOperationException();

        //    wallet.AvailableBalance += dto.AvailableBalance;
        //    await _walletsRepository.UpdateWallet(wallet);

        //}

        public async Task<WalletDto> GetWallet(int userId, int walletId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            var wallet = await _walletsRepository.GetWallet(walletId);

            if (wallet == null)
                throw new InvalidOperationException();

            return new WalletDto()
            {
                WalletId = walletId,
                AvailableBalance = wallet.Availablebalance,
                CreatedAt = wallet.Createdat,
                Role = (WalletRoleType)wallet.Userwallets.FirstOrDefault(u => u.Userid == userId)!.Walletroleid,
                SavedBalance = wallet.Savedbalance,
                WalletName = wallet.Walletname,
            };
        }

        public async Task<ICollection<WalletDto>> GetWalletsByUserId(int userId)
        {
            var wallets = await _walletsRepository.GetWalletsByUserId(userId);
            return wallets.Select(w => new WalletDto()
            {
                WalletId = w.Walletid,
                AvailableBalance = w.Availablebalance,
                CreatedAt = w.Createdat,
                SavedBalance = w.Savedbalance,
                Role = (WalletRoleType)w.Userwallets
        .FirstOrDefault(uw => uw.Userid == userId)!.Walletroleid,
                WalletName = w.Walletname,
            }).ToList();
        }

        public async Task SaveBalance(int userId, int walletId, UpdateWalletSavedBalanceDto walletDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);


            var wallet = await _walletsRepository.GetWallet(walletId);

            if (wallet == null)
                throw new InvalidOperationException();

            if (wallet.Availablebalance < walletDto.TransferBalance)
                throw new InvalidOperationException("Not Enough Balance To Transfer!");

            wallet.Availablebalance -= walletDto.TransferBalance;
            wallet.Savedbalance += walletDto.TransferBalance;


            await _walletsRepository.UpdateWallet(wallet);


        }

        public async Task UpdateWalletName(int userId, int walletId, UpdateWalletNameDto walletDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Owner);


            var wallet = await _walletsRepository.GetWallet(walletId);


            if (wallet == null)
                throw new KeyNotFoundException("Wallet Doesn't Exist!");



            wallet.Walletname = walletDto.WalletName;

            await _walletsRepository.UpdateWallet(wallet);

        }

        public async Task TransferSavedBalanceToAvailableBalance(int userId, int walletId, TransferSavedBalanceToAvailableBalanceDto walletDto)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Editor);


            var wallet = await _walletsRepository.GetWallet(walletId);

            if (wallet == null)
                throw new KeyNotFoundException("Wallet Doesn't Exist!");
            if (wallet.Savedbalance < walletDto.TransferBalance)
                throw new InvalidOperationException("Not Enough Balance To Transfer!");

            wallet.Savedbalance -= walletDto.TransferBalance;
            wallet.Availablebalance += walletDto.TransferBalance;

            await _walletsRepository.UpdateWallet(wallet);
        }

        public async Task AddUserToWallet(int userId, int walletId, int targetUserId, WalletRoleType role = WalletRoleType.Viewer)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Owner);

            if (role == WalletRoleType.Owner)
                throw new InvalidOperationException("Cannot make another user the owner of your wallet!");

            var doesUserExist = await _usersRepository.GetUser(targetUserId);
            if (doesUserExist == null)
                throw new KeyNotFoundException("Target User Doesn't Exist");

            var userWallet = new Userwallet()
            {
                Userid = targetUserId,
                Walletid = walletId
                ,
                Walletroleid = (byte)role
            };
            await _userWalletsRepository.CreateUserWallet(userWallet);
        }

        public async Task RemoveUserFromWallet(int userId, int walletId, int targetUserId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Owner);

            await _userWalletsRepository.DeleteUserWallet(targetUserId, walletId);


        }

        public async Task LeaveWallet(int userId, int walletId)
        {
            var userWallet = await _userWalletsRepository.GetUserWalletByUserIdAndWalletId(userId, walletId);

            if (userWallet == null)
                throw new KeyNotFoundException("You aren't connected to that wallet!");

            if (userWallet.Walletroleid == (byte)WalletRoleType.Owner)
                throw new InvalidOperationException("Owner cannot leave the wallet");

            await _userWalletsRepository.DeleteUserWallet(userId, walletId);

        }

        public async Task<ICollection<UsersConnectedToWalletDto>> GetUsersConnectedToWallet(int userId, int walletId)
        {
            await _walletRoleGuard.Authorize(userId, walletId, WalletRoleType.Viewer);

            var userWallets = await _userWalletsRepository.GetUserWalletsByWalletId(walletId);


            return userWallets.Select(uw => new UsersConnectedToWalletDto()
            {
                UserId = uw.User.Userid,
                Username = uw.User.Username,
                Role = ((WalletRoleType)uw.Walletroleid).ToString(),
            }).ToList();
        }

        public async Task<WalletDto> GetWalletByOwnerRole(int userId)
        {
            var wallet = await _walletsRepository.GetWalletByOwnerRole(userId);

            if (wallet == null)
                throw new KeyNotFoundException("User Doesn't have a wallet!");

            await _walletRoleGuard.Authorize(userId, wallet.Walletid, WalletRoleType.Owner);


            return new WalletDto()
            {
                WalletId = wallet.Walletid,
                AvailableBalance = wallet.Availablebalance,
                CreatedAt = wallet.Createdat,
                SavedBalance = wallet.Savedbalance,
                WalletName = wallet.Walletname,
            };
        }
    }
}
