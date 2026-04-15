using FinanceTracker.Dtos.LoginDto;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinanceTracker.Service
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;

        private readonly IUsersRepository _usersRepository;

        private readonly IActiveUserTrackerService _activeUserTrackerService;
        public AuthService(IUsersRepository userRepository, IConfiguration configuration, IActiveUserTrackerService activeUserTrackerService)

        {
            _configuration = configuration;
            _usersRepository = userRepository;
            _activeUserTrackerService = activeUserTrackerService;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto authUser)
        {
            var user = await _usersRepository.GetUserByEmail(authUser.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Wrong credentials");


            if (user.Isactive == false)
                throw new UnauthorizedAccessException("Verify your email");


            bool isValidPassword = BCrypt.Net.BCrypt.Verify(authUser.Password, user.Passwordhash);

            if (!isValidPassword)
                // We don't tell which one is wrong for security reasons
                throw new UnauthorizedAccessException("Wrong credentials");



            var claims = new[] {


                new Claim(ClaimTypes.NameIdentifier,user.Userid.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),

                            new Claim(ClaimTypes.Email,user.Email.ToString())

                            // Can add Roles if needed later


            };


            var secretKey = _configuration["JWT_SECRET_KEY"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(



                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:ExpirationMinutes"]))


                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = GenerateRefreshToken();

            user.Refreshtokenhash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
            user.Refreshtokenexpiresat = DateTime.UtcNow.AddDays(7);

            user.Refreshtokenrevokedat = null;

            await _usersRepository.UpdateUser(user);

            _activeUserTrackerService.Track(user.Userid);

            return new LoginResponseDto
            {
                UserId = user.Userid,
                Email = user.Email,
                RefreshToken = refreshToken,
                Token = tokenString,
                Username = user.Username,


            };



        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public async Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto refreshRequest)
        {

            var user = await _usersRepository.GetUserByEmail(refreshRequest.Email);

            if (user == null)
                throw new UnauthorizedAccessException();


            //User Logged out so u cant refresh Token
            if (user.Refreshtokenrevokedat != null)
                throw new UnauthorizedAccessException();

            //Refresh Token Expired
            if (user.Refreshtokenexpiresat <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException();

            }
            //User didn't even try logging in
            if (user.Refreshtokenhash == null)
            {
                throw new UnauthorizedAccessException();

            }

            bool isValidRefreshToken = BCrypt.Net.BCrypt
                .Verify(refreshRequest.RefreshToken, user.Refreshtokenhash);


            //Wrong Refresh Token!
            if (!isValidRefreshToken)
            {
                throw new UnauthorizedAccessException();

            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Userid.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                                    new Claim(ClaimTypes.Name, user.Username),

            };

            var secretKey = _configuration["JWT_SECRET_KEY"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var signCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(


                issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                      expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:ExpirationMinutes"])),
                signingCredentials: signCreds

                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


            var refreshToken = GenerateRefreshToken();


            user.Refreshtokenhash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            user.Refreshtokenexpiresat = DateTime.UtcNow.AddDays(7);

            await _usersRepository.UpdateUser(user);

            _activeUserTrackerService.ClearStale(TimeSpan.FromMinutes(30));
            _activeUserTrackerService.Track(user.Userid);


            return new RefreshTokenResponseDto()
            {
                Token = tokenString,
                RefreshToken = refreshToken,
            };
        }
        public async Task Logout(LogoutRequestDto logoutRequest)
        {
            var user = await _usersRepository.GetUserByEmail(logoutRequest.Email);

            if (user == null)
                throw new UnauthorizedAccessException();


            bool isRefreshTokenValid = BCrypt.Net.BCrypt.Verify(logoutRequest.RefreshToken, user.Refreshtokenhash);
            if (!isRefreshTokenValid)
                throw new UnauthorizedAccessException();

            user.Refreshtokenrevokedat = DateTime.UtcNow;

            await _usersRepository.UpdateUser(user);

        }
    }
}
