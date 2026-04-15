
using FinanceTracker.Dtos.LoginDto;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;
using FinanceTracker.Service;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;


namespace FinanceTracker.Tests
{
    public class AuthServiceTests
    {

        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IUsersRepository> _usersRepositoryMoq;
        private readonly Mock<IActiveUserTrackerService> _activeUserTrackerMoq;

        private readonly IAuthService _service;
        public AuthServiceTests()
        {


            _usersRepositoryMoq = new Mock<IUsersRepository>();

            _activeUserTrackerMoq = new Mock<IActiveUserTrackerService>();

            _configMock = new Mock<IConfiguration>();


            _configMock.Setup(c => c["JWT_SECRET_KEY"])
      .Returns("super_secret_key_123456789_super_secure_long_key_12345");

            _configMock.Setup(c => c["JwtSettings:Issuer"])
                .Returns("test_issuer");

            _configMock.Setup(c => c["JwtSettings:Audience"])
                .Returns("test_audience");

            _configMock.Setup(c => c["JwtSettings:ExpirationMinutes"])
                .Returns("60");



            _service = new AuthService(
                _usersRepositoryMoq.Object,
                _configMock.Object,
                _activeUserTrackerMoq.Object
            );



        }


        [Fact]
        public async Task Login_ValidCredentials_ReturnsLoginResponseDto()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = true,
                    Isactive = true
                });

            var dto = new LoginRequestDto
            {

                Email = "test@gmail.com"
        ,
                Password = "Password123"

            };

            //Act
            var result = await _service.Login(dto);

            //Assert

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.Email.Should().Be("test@gmail.com");
        }


        [Fact]
        public async Task Login_UserNotFound_ThrowUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("wrong@gmail.com"))
                .ReturnsAsync((User?)null);

            var dto = new LoginRequestDto
            {

                Email = "wrong@gmail.com"
        ,
                Password = "Password123"

            };

            //Act
            Func<Task> act = async () => await _service.Login(dto);

            //Assert

            // very strict
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Wrong credentials");


        }

        [Fact]
        public async Task Login_UserNotActive_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = true,
                    Isactive = false
                });

            var dto = new LoginRequestDto
            {

                Email = "test@gmail.com"
        ,
                Password = "Password123"

            };

            //Act
            Func<Task> act = async () => await _service.Login(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();


        }

        [Fact]
        public async Task Login_WrongPassword_ThrowUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = true,
                    Isactive = true
                });

            var dto = new LoginRequestDto
            {

                Email = "Test@gmail.com"
        ,
                Password = "WrongPassword123"

            };

            //Act
            Func<Task> act = async () => await _service.Login(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Wrong credentials");


        }

        [Fact]
        public async Task Login_EmailNotVerified_ThrowUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = false,
                    Isactive = true
                });

            var dto = new LoginRequestDto
            {

                Email = "Test@gmail.com"
        ,
                Password = "Password123"

            };

            //Act
            Func<Task> act = async () => await _service.Login(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();


        }






        [Fact]
        public async Task RefreshToken_ValidRefresh_ReturnsRefreshTokenResponseDto()
        {
            //Arrange
            var realToken = "testrefreshtoken123testrefreshtoken123testrefreshtoken123testtoken";


            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = true,
                    Isactive = true,
                    Refreshtokenhash = BCrypt.Net.BCrypt.HashPassword(realToken),
                    Refreshtokenrevokedat = null,
                    Refreshtokenexpiresat = DateTime.UtcNow.AddDays(7)

                });


            var dto = new RefreshTokenRequestDto
            {

                Email = "test@gmail.com",
                RefreshToken = realToken


            };

            //Act
            var result = await _service.RefreshToken(dto);

            //Assert

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public async Task RefreshToken_UserNotFound_ThrowUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("wrong@gmail.com"))
                .ReturnsAsync((User?)null);


            var dto = new RefreshTokenRequestDto
            {

                Email = "wrong@gmail.com",


            };

            //Act
            Func<Task> act = async () => await _service.RefreshToken(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();

        }

        [Fact]
        public async Task RefreshToken_RefreshTokenExpired_ThrowUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = true,
                    Isactive = true,
                    Refreshtokenrevokedat = null,
                    Refreshtokenexpiresat = DateTime.UtcNow.AddDays(-1)

                });


            var dto = new RefreshTokenRequestDto
            {

                Email = "test@gmail.com",


            };

            //Act
            Func<Task> act = async () => await _service.RefreshToken(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();

        }


        [Fact]

        public async Task RefreshToken_RefreshTokenRevoked_ThrowUnauthorizedAccessException()
        {
            //Arrange
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com",
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Emailverified = true,
                    Isactive = true,
                    Refreshtokenrevokedat = DateTime.UtcNow,


                });


            var dto = new RefreshTokenRequestDto
            {

                Email = "test@gmail.com",


            };

            //Act
            Func<Task> act = async () => await _service.RefreshToken(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();

        }



        [Fact]

        public async Task RefreshToken_NotValidRefreshToken_ThrowUnauthorizedAccessException()
        {
            //Arrange
            var realToken = "testrefreshtoken123testrefreshtoken123testrefreshtoken123testtoken";
            var wrongToken = "wrongtoken123wrongtoken123wrongtoken123wrongtoken123wrongtoken123";
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(new User()
                {
                    Userid = 1,
                    Username = "Test",
                    Email = "test@gmail.com"
                ,
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword("Password123")
                ,
                    Emailverified = true
                ,
                    Isactive = true
                    ,
                    Refreshtokenrevokedat = null,
                    Refreshtokenexpiresat = DateTime.UtcNow.AddDays(1),
                    Refreshtokenhash = BCrypt.Net.BCrypt.HashPassword(realToken),


                });


            var dto = new RefreshTokenRequestDto
            {

                Email = "test@gmail.com",
                RefreshToken = wrongToken


            };

            //Act
            Func<Task> act = async () => await _service.RefreshToken(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();

        }

        [Fact]
        public async Task Logout_ValidLogout_UpdatesUser()
        {
            //Arrange
            var realToken = "testrefreshtoken123testrefreshtoken123testrefreshtoken123testtoken";

            var user = new User()
            {
                Userid = 1,
                Username = "Test",
                Email = "test@gmail.com",
                Refreshtokenrevokedat = null,
                Refreshtokenhash = BCrypt.Net.BCrypt.HashPassword(realToken),
                
            };

            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(user);




            var dto = new LogoutRequestDto
            {

                Email = "test@gmail.com",
                RefreshToken = realToken


            };

            //Act
            await _service.Logout(dto);

            //Assert

            user.Refreshtokenrevokedat.Should().NotBeNull();
            _usersRepositoryMoq.Verify(u => u.UpdateUser(It.IsAny<User>()), Times.Once());

        }

        [Fact]
        public async Task Logout_UserNotFound_ThrowUnauthorizedAccessException()
        {
            //Arrange

     

            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync((User?)null);




            var dto = new LogoutRequestDto
            {

                Email = "test@gmail.com",
                RefreshToken = "Not Needed For Test"


            };

            //Act

          var act = async () =>  await _service.Logout(dto);

            //Assert

     await   act.Should().ThrowAsync<UnauthorizedAccessException>();

        }


        [Fact]
        public async Task Logout_NotValidRefreshToken_ThrowUnauthorizedAccessException()
        {
            //Arrange
            var realToken = "testrefreshtoken123testrefreshtoken123testrefreshtoken123testtoken";

            var user = new User()
            {
                Email = "test@gmail.com",
                Refreshtokenhash = BCrypt.Net.BCrypt.HashPassword(realToken),

            };
            _usersRepositoryMoq.Setup(u => u.GetUserByEmail("test@gmail.com"))
                .ReturnsAsync(user);




            var dto = new LogoutRequestDto
            {

                Email = "test@gmail.com",
                RefreshToken = "Wrong token",


            };

            //Act

            var act = async () => await _service.Logout(dto);

            //Assert

            await act.Should().ThrowAsync<UnauthorizedAccessException>();

        }


    }
}
