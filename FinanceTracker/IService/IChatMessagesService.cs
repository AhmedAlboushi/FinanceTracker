using FinanceTracker.Dtos.ChatMessageDto;

namespace FinanceTracker.IService
{
    public interface IChatMessagesService
    {

        public Task<ICollection<ChatMessageDto>> GetChatMessagesBySenderIdAndRecieverId(int senderId, int receiverId, int page = 1, int pageSize = 50);

        public Task SendChatMessage(int senderId, int receiverId, CreateChatMessageDto chatMessageDto);

        public Task MarkAllMessagesAsRead(int senderId, int receiverId);

        public Task<ICollection<ChatMessageDto>> GetNewMessages(int senderId, int receiverId, int lastMessageId);

    }
}
