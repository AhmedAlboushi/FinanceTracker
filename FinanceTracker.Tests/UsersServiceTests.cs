using FinanceTracker.Dtos.UserDto;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;
using FinanceTracker.Service;
using FluentAssertions;
using Moq;
using System.Security.Cryptography;


namespace FinanceTracker.Tests
{
 
    public class UsersServiceTests
    {
       private readonly Mock<IWalletsRepository> _walletsRepositoryMoq;
        private readonly Mock<IUsersRepository> _usersRepositoryMoq;
        private readonly Mock<IEmailService> _emailServiceMoq;
        private readonly Mock<IUserWalletsRepository> _userWalletsRepositoryMoq;

        private readonly IUsersService _service;

        public UsersServiceTests()
        {
            _walletsRepositoryMoq = new Mock<IWalletsRepository>();
            _usersRepositoryMoq = new Mock<IUsersRepository>();
            _emailServiceMoq = new Mock<IEmailService>();
            _userWalletsRepositoryMoq = new Mock<IUserWalletsRepository>();

            _service = new UsersService(_usersRepositoryMoq.Object,
                _emailServiceMoq.Object, _walletsRepositoryMoq.Object, _userWalletsRepositoryMoq.Object);

        }


        [Fact]

        public async Task CreateUser_EmailAlreadyExists_ThrowsInvalidOperationException()
        {
            //ARRANGE
            var userDto = new CreateUserDto()
            {
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Test123"

            };

            _usersRepositoryMoq.Setup(u => u.DoesUserExistByEmail(userDto.Email))
                .ReturnsAsync(true);
            //ACT

            Func <Task> act = async () => await _service.CreateUser(userDto);
            //ASSERT;

            await act.Should().ThrowAsync<InvalidOperationException>(); 
        }

        [Fact]

        public async Task CreateUser_UsernameAlreadyExists_ThrowsInvalidOperationException()
        {
            //ARRANGE
            var userDto = new CreateUserDto()
            {
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Test123"

            };

            _usersRepositoryMoq.Setup(u => u.DoesUserExistByUsername(userDto.Username))
                .ReturnsAsync(true);
            //ACT

            Func<Task> act = async () => await _service.CreateUser(userDto);
            //ASSERT;

            await act.Should().ThrowAsync<InvalidOperationException>();
        }


        [Fact]
        public async Task CreateUser_FailedToCreateUser_ThrowsException()
        {
            // ARRANGE
            var userDto = new CreateUserDto()
            {
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Test123"
            };

            _usersRepositoryMoq
                .Setup(u => u.CreateUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Failed To Create User"));

            // ACT
            Func<Task> act = async () => await _service.CreateUser(userDto);

            // ASSERT
            await act.Should().ThrowAsync<Exception>();
            _emailServiceMoq.Verify(
    e => e.SendEmail(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()),
    Times.Never()
);
        }

        [Fact]

        public async Task CreateUser_UserCreated_CreatesUser()
        {
            //ARRANGE

            var userDto = new CreateUserDto()
            {
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Test123"

            };
    
            //ACT

           await _service.CreateUser(userDto);
            //ASSERT;

            _usersRepositoryMoq.Verify(u => u.CreateUser(It.IsAny<User>()), Times.Once());

        }

        [Fact]

        public async Task CreateUser_EmailConfirmationSent_CreatesUser()
        {
            //ARRANGE

            var userDto = new CreateUserDto()
            {
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Test123"

            };

            //ACT

            await _service.CreateUser(userDto);
            //ASSERT;

            _emailServiceMoq.Verify(u => u.SendEmail(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()),
                Times.Once());
        }
    }
}
