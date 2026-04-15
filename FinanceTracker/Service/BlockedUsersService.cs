using FinanceTracker.Dtos.BlockedUserDto;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class BlockedUsersService : IBlockedUsersService
    {
        private readonly IBlockedUsersRepository _blockedUsersRepository;
        private readonly IUsersRepository _usersRepository;
        public BlockedUsersService(IBlockedUsersRepository blockedUsersRepository, IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _blockedUsersRepository = blockedUsersRepository;
        }

        public async Task BlockUser(int userId, int targetUserId)
        {

            var user = await _usersRepository.GetUser(userId);

            if (user == null)
                throw new KeyNotFoundException("User Doesn't Exist");

            var targetUser = await _usersRepository.GetUser(targetUserId);


            if (targetUser == null)
                throw new KeyNotFoundException("Target User Doesn't Exist");
            if (targetUserId == userId)
                throw new KeyNotFoundException("Cannot block yourself");



            var blockedUser = new Blockeduser()
            {
                Createdat = DateOnly.FromDateTime(DateTime.UtcNow),
                Targetuserid = targetUserId,
                Userid = userId,
            };
            await _blockedUsersRepository.CreateBlockedUser(blockedUser);
        }

        public async Task<ICollection<BlockedUserDto>> GetBlockedUsersByUserId(int userId)
        {
            var blockedUsersDto = await _blockedUsersRepository.GetBlockedUsersByUserId(userId);

            return blockedUsersDto.Select(b => new BlockedUserDto()
            {
                TargetUsername = b.Targetuser.Username,
                TargetUserId = b.Targetuserid
                ,
                UserId = b.Userid,
                CreatedAt = b.Createdat,

            }).ToList();

        }

        public async Task UnBlockUser(int userId, int targetUserId)
        {

            var blockedUser = await _blockedUsersRepository.GetBlockedUser(userId, targetUserId);

            if (blockedUser == null)
                throw new KeyNotFoundException("Target User Isn't blocked");


            await _blockedUsersRepository.DeleteBlockedUser(userId, targetUserId);
        }
    }
}
