using FinanceTracker.Models;


namespace FinanceTracker.IRepository
{
    public interface IChatMessagesRepository
    {

        public Task<ICollection<Chatmessage>> GetChatMessagesBySenderIdAndRecieverId(int senderId, int recieverId, int page = 1, int pageSize = 50);

        public Task CreateChatMessage(Chatmessage chatMessage);

        public Task MarkAllMessagesAsRead(int senderId, int recieverId);

        public Task<ICollection<Chatmessage>> GetNewMessages(int senderId, int receiver, int lastMessageId);
    }
}
