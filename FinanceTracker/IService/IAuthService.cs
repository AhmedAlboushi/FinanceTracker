using FinanceTracker.Dtos.LoginDto;

namespace FinanceTracker.IService
{
    public interface IAuthService
    {

        public Task<LoginResponseDto> Login(LoginRequestDto loginDto);

        public Task Logout(LogoutRequestDto logoutRequestDto);

        public Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
    }
}
