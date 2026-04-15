using FinanceTracker.Dtos.UserDto;
using FinanceTracker.Filters;
using FinanceTracker.IService;
using FinanceTracker.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("GeneralLimiter")]

    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IActiveUserTrackerService _activeUserTrackerService;
        public UsersController(IUsersService usersService, IActiveUserTrackerService activeUserTrackerService)
        {
            _activeUserTrackerService = activeUserTrackerService;
            _usersService = usersService;
        }

        [EnableRateLimiting("GeneralLimiter")]
        [Authorize]
        [HttpGet("active-users")]

        public IActionResult GetActiveUsers()
        {
            var activeUsers = _activeUserTrackerService.GetActiveUsers();

            return Ok(activeUsers);
        }

        [Authorize]

        [HttpGet("{userId}")]

        public async Task<IActionResult> GetUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _usersService.GetUserById(userId);

            return Ok(user);
        }


        [LogAction("Register", 3)]
        [HttpPost("register")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> RegisterUser(CreateUserDto userDto)
        {

            await _usersService.CreateUser(userDto);



            return Ok("Verify Your Email! we have sent you a link");



        }

        [Authorize]
        [HttpPut]
        [EnableRateLimiting("AuthLimiter")]
        [LogAction("ChangeUsername", 1)]

        public async Task<IActionResult> ChangeUsername(UpdateUserDto userDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _usersService.UpdateUser(userId, userDto);

            return Ok("Username changed Succesfully!");
        }

        [HttpPost("email-verification")]
        [EnableRateLimiting("AuthLimiter")]
        [LogAction("EmailVerification", 3)]

        public async Task<IActionResult> VerifyEmailAndInitializeAccount([FromQuery] string email, [FromQuery] string token)
        {


            await _usersService.VerifyEmailAndInitializeAccount(email, token);
            return Ok("Verified Succesfully!");




        }
        [LogAction("ResendVerificationEmail", 3)]

        [HttpPost("resend-email-verification")]
        [EnableRateLimiting("AuthLimiter")]

        public async Task<IActionResult> ResendVerificationEmail([FromQuery] string email)
        {
            await _usersService.ResendVerificationEmail(email);

            return Ok("Verification Email Resent!");
        }
    }
}
