using FinanceTracker.Dtos.UserDto;
using FinanceTracker.Enums;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;
using System.Security.Cryptography;

namespace FinanceTracker.Service
{
    public class UsersService : IUsersService
    {
        private readonly IWalletsRepository _walletsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailService _emailService;
        private readonly IUserWalletsRepository _userWalletsRepository;
        public UsersService(IUsersRepository usersRepository, IEmailService emailService,
            IWalletsRepository walletsRepository, IUserWalletsRepository userWalletsRepository)
        {
            _usersRepository = usersRepository;
            _emailService = emailService;
            _walletsRepository = walletsRepository;
            _userWalletsRepository = userWalletsRepository;
        }


        public async Task<ICollection<UserDto>> GetUsers()
        {
            var users = await _usersRepository.GetUsers();

            return users.Select(user => new UserDto()
            {
                UserId = user.Userid,
                CreatedAt = user.Createdat,
                IsActive = user.Isactive,
                Email = user.Email,
                Username = user.Username


            }).ToList();

        }

        public async Task<UserDto> GetUserById(int userId)
        {
            var user = await _usersRepository.GetUser(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User Doesn't Exist");
            }

            return new UserDto()
            {
                UserId = userId,
                CreatedAt = user.Createdat,
                IsActive = user.Isactive,
                Email = user.Email,
                Username = user.Username
            };
        }

        public async Task<UserDto> GetUserByUsername(string username)
        {
            var user = await _usersRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new KeyNotFoundException("User Doesn't Exist");
            }

            return new UserDto()
            {
                UserId = user.Userid,
                CreatedAt = user.Createdat,
                IsActive = user.Isactive,
                Email = user.Email,
                Username = user.Username
            };
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _usersRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new KeyNotFoundException("User Doesn't Exist");
            }

            return new UserDto()
            {
                UserId = user.Userid,
                CreatedAt = user.Createdat,
                IsActive = user.Isactive,
                Email = user.Email,
                Username = user.Username
            };

        }

        public async Task CreateUser(CreateUserDto userDto)
        {
            if (await _usersRepository.DoesUserExistByEmail(userDto.Email))
                throw new InvalidOperationException("Email Already Exists!");

            if (await _usersRepository.DoesUserExistByUsername(userDto.Username))
                throw new InvalidOperationException("Username already Exists");

            var emailVerificationTokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailVerificationTokenString = Convert.ToBase64String(emailVerificationTokenBytes);

            var user = new User()
            {
                Username = userDto.Username,
                Isactive = false,
                Email = userDto.Email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Emailverificationtoken = emailVerificationTokenString,
                Emailverificationexpiresat = DateTime.UtcNow.AddHours(24),
                Createdat = DateOnly.FromDateTime(DateTime.UtcNow), // in database by default its on user creation


            };

            await _usersRepository.CreateUser(user);


            var userId = user.Userid;
            await _emailService.SendEmail(user.Email, userId, emailVerificationTokenString);
        }

        public async Task ResendVerificationEmail(string email)
        {

            var user = await _usersRepository.GetUserByEmail(email);

            if (user == null)
                throw new KeyNotFoundException("User Doesn't Exist with that email");

            if (user.Emailverified == true)
                throw new InvalidOperationException("Email Already Verified!");

            var emailVerificationTokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailVerificationTokenString = Convert.ToBase64String(emailVerificationTokenBytes);

            user.Emailverificationtoken = emailVerificationTokenString;
            user.Emailverificationexpiresat = DateTime.UtcNow.AddHours(24);

            await _usersRepository.UpdateUser(user);
            await _emailService.SendEmail(user.Email, user.Userid, emailVerificationTokenString);

        }

        public async Task UpdateUser(int userId, UpdateUserDto userDto)
        {
            var user = await _usersRepository.GetUser(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User Doesn't Exist");
            }


            user.Username = userDto.Username;

            await _usersRepository.UpdateUser(user);


        }
        private async Task CreateDefaultWalletForUser(int userId)
        {
            var wallet = new Wallet { Walletname = "My Wallet" };
            await _walletsRepository.CreateWallet(wallet);

            var userWallet = new Userwallet
            {
                Userid = userId,
                Walletid = wallet.Walletid,
                Walletroleid = (short)WalletRoleType.Owner
            };
            await _userWalletsRepository.CreateUserWallet(userWallet);
        }
        public async Task VerifyEmailAndInitializeAccount(string email, string token)
        {
            var user = await _usersRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new KeyNotFoundException("User Doesn't Exist");


            }


            if (user.Emailverificationtoken != token && user.Emailverificationexpiresat < DateTime.UtcNow)
            {
                throw new InvalidCastException();
            }

            user.Isactive = true;
            user.Emailverificationtoken = null;
            user.Emailverified = true;
            user.Emailverificationexpiresat = null;

            await _usersRepository.UpdateUser(user);

            await CreateDefaultWalletForUser(user.Userid); // Creates and links wallet to user

        }
    }
}
