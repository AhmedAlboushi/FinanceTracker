using FinanceTracker.Dtos.LoginDto;
using FinanceTracker.Filters;
using FinanceTracker.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.RateLimiting;


namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [LogAction("Login", 3)]
        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]

        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            var loginResponseDto = await _authService.Login(loginDto);



            return Ok(loginResponseDto);
        }

        [Authorize]
        [EnableRateLimiting("AuthLimiter")]

        [HttpPost("Logout")]

        public async Task<IActionResult> Logout(LogoutRequestDto logoutRequestDto)
        {
            await _authService.Logout(logoutRequestDto);

            return Ok();
        }

        [LogAction("RefreshRequest", 3)]

        [HttpPost("refresh-token")]
        [EnableRateLimiting("AuthLimiter")]

        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {

            var refreshTokenResponse = await _authService.RefreshToken(refreshTokenRequestDto);


            return Ok(refreshTokenResponse);
        }
    }
}
