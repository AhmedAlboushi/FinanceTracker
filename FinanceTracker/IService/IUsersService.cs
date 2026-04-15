using FinanceTracker.Dtos.UserDto;

namespace FinanceTracker.IService
{
    public interface IUsersService
    {

        Task<ICollection<UserDto>> GetUsers();

        Task<UserDto> GetUserById(int userId);

        Task<UserDto> GetUserByUsername(string username);

        Task<UserDto> GetUserByEmail(string email);

        Task CreateUser(CreateUserDto userDto);

        Task UpdateUser(int userId, UpdateUserDto userDto);
        Task VerifyEmailAndInitializeAccount(string email, string token);

        Task ResendVerificationEmail(string email);
    }
}
