using FinanceTracker.Dtos.WalletDto;
using FinanceTracker.Enums;
using FinanceTracker.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("GeneralLimiter")]

    public class WalletsController : ControllerBase
    {
        private readonly IWalletsService _walletsService;

        public WalletsController(IWalletsService walletsService)
        {
            _walletsService = walletsService;
        }
        [HttpGet("wallet-by-owner")]

        public async Task<IActionResult> GetWalletByOwnerRole()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var walletDto = await _walletsService.GetWalletByOwnerRole(userId);

            return Ok(walletDto);
        }

        [HttpGet("{walletId}")]

        public async Task<IActionResult> GetWallet(int walletId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var walletDto = await _walletsService.GetWallet(userId, walletId);

            return Ok(walletDto);
        }

        [HttpGet("by-user")]

        public async Task<IActionResult> GetWalletByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var walletsDto = await _walletsService.GetWalletsByUserId(userId);

            return Ok(walletsDto);
        }
        [HttpGet("{walletId}/users")]

        public async Task<IActionResult> GetUsersConnectedToWallet(int walletId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var connectedUsers = await _walletsService.GetUsersConnectedToWallet(userId, walletId);

            return Ok(connectedUsers);

        }

        [HttpPut("{walletId}/save-balance")]

        public async Task<IActionResult> SaveBalance(int walletId, UpdateWalletSavedBalanceDto walletDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _walletsService.SaveBalance(userId, walletId, walletDto);

            return Ok("Balance Saved Succesfully!");
        }

        [HttpPut("withdraw-balance/{walletId}")]

        public async Task<IActionResult> TransferSavedBalanceToAvailableBalance(int walletId,
            TransferSavedBalanceToAvailableBalanceDto walletDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _walletsService.TransferSavedBalanceToAvailableBalance(userId, walletId, walletDto);

            return Ok("Transfered Succesfully!");
        }

        [HttpPut("{walletId}/name")]
        public async Task<IActionResult> UpdateWalletName(int walletId, UpdateWalletNameDto walletDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _walletsService.UpdateWalletName(userId, walletId, walletDto);

            return Ok("Name Updated Succesfully!");
        }

        [HttpPost("{walletId}/user/{targetUserId}")]

        public async Task<IActionResult> AddUserToWallet(int walletId, int targetUserId, WalletRoleType role)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _walletsService.AddUserToWallet(userId, walletId, targetUserId, role);

            return Ok("User Added To wallet Succesfully!");
        }

        [HttpDelete("{walletId}/user/{targetUserId}")]

        public async Task<IActionResult> RemoveUserFromWallet(int walletId, int targetUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _walletsService.RemoveUserFromWallet(userId, walletId, targetUserId);

            return Ok("User Removed from wallet succesfully!");
        }

        [HttpDelete("{walletId}/leave")]

        public async Task<IActionResult> LeaveWallet(int walletId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _walletsService.LeaveWallet(userId, walletId);

            return Ok("Left wallet succesfully!");
        }

    }
}
