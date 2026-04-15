using FinanceTracker.Dtos.ChatMessageDto;
using FinanceTracker.Enums;
using FinanceTracker.Helpers;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Models;

namespace FinanceTracker.Service
{
    public class ChatMessagesService : IChatMessagesService
    {
        private readonly IChatMessagesRepository _chatMessagesRepository;
        private readonly IFriendshipsRepository _friendshipsRepository;
        private readonly IBlockedUsersRepository _blockedUsersRepository;
        public ChatMessagesService(IChatMessagesRepository chatMessagesRepository, IFriendshipsRepository friendshipsRepository
                    , IBlockedUsersRepository blockedUsersRepository)
        {
            _friendshipsRepository = friendshipsRepository;
            _chatMessagesRepository = chatMessagesRepository;
            _blockedUsersRepository = blockedUsersRepository;
        }
        public async Task<ICollection<ChatMessageDto>> GetChatMessagesBySenderIdAndRecieverId(int senderId, int receiverId,
            int page = 1, int pageSize = 50)
        {
            (page, pageSize) = PaginationHelper.Validate(page, pageSize);

            var friendShip = await _friendshipsRepository.GetFriendship(senderId, receiverId);

            if (friendShip == null)
                throw new InvalidOperationException("You aren't friends with this user!");



            var chatMessages = await _chatMessagesRepository.GetChatMessagesBySenderIdAndRecieverId(senderId, receiverId, page, pageSize);

            return chatMessages.Select(c => new ChatMessageDto()
            {
                CreatedAt = c.Createdat,
                Content = c.Content,
                IsRead = c.Isread,
                MessageId = c.Messageid,
                ReceiverUserId = c.Receiveruserid,
                SenderUserId = c.Senderuserid,
            }).ToList();
        }

        public async Task<ICollection<ChatMessageDto>> GetNewMessages(int senderId, int receiverId, int lastMessageId)
        {

            var friendShip = await _friendshipsRepository.GetFriendship(senderId, receiverId);

            if (friendShip == null)
                throw new InvalidOperationException("You aren't friends with this user!");



            var chatMessages = await _chatMessagesRepository.GetNewMessages(senderId, receiverId, lastMessageId);

            return chatMessages.Select(c => new ChatMessageDto()
            {
                CreatedAt = c.Createdat,
                Content = c.Content,
                IsRead = c.Isread,
                MessageId = c.Messageid,
                ReceiverUserId = c.Receiveruserid,
                SenderUserId = c.Senderuserid,
            }).ToList();
        }

        public async Task MarkAllMessagesAsRead(int senderId, int receiverId)
        {
            await _chatMessagesRepository.MarkAllMessagesAsRead(senderId, receiverId);
        }

        public async Task SendChatMessage(int senderId, int receiverId, CreateChatMessageDto chatMessageDto)
        {

            var friendShip = await _friendshipsRepository.GetFriendship(senderId, receiverId);

            if (friendShip == null || friendShip.Status != (byte)FriendshipStatus.Accepted)
                throw new InvalidOperationException("You aren't friends with this user!");


            var isBlocked = await _blockedUsersRepository.IsBlocked(senderId, receiverId);
            if (isBlocked)
                throw new InvalidOperationException("You blocked or got blocked by this user");


            var chatMessage = new Chatmessage()
            {
                Createdat = DateTime.UtcNow,
                Content = chatMessageDto.Content,
                Isread = false,
                Receiveruserid = receiverId,
                Senderuserid = senderId,

            };
            await _chatMessagesRepository.CreateChatMessage(chatMessage);
        }


    }
}
