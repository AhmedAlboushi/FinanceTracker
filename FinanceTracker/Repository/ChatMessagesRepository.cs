using FinanceTracker.Data;
using FinanceTracker.IRepository;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Repository
{
    public class ChatMessagesRepository : IChatMessagesRepository
    {
        private readonly FinanceTrackerDbContext _context;
        public ChatMessagesRepository(FinanceTrackerDbContext context)

        {
            _context = context;
        }
        public async Task CreateChatMessage(Chatmessage chatMessage)
        {
            await _context.Chatmessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Chatmessage>> GetChatMessagesBySenderIdAndRecieverId(
        int senderId, int receiverId, int page = 1, int pageSize = 50)
        {
            var latestMessages = await _context.Chatmessages
          .Where(c => (c.Senderuserid == senderId && c.Receiveruserid == receiverId) ||
                      (c.Senderuserid == receiverId && c.Receiveruserid == senderId))
          .OrderByDescending(c => c.Createdat)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

            return latestMessages.OrderBy(m => m.Createdat).ToList();
        }

        public async Task<ICollection<Chatmessage>> GetNewMessages(int senderId, int receiverId, int lastMessageId)
        {
            return await _context.Chatmessages
        .Where(m =>
            ((m.Senderuserid == senderId && m.Receiveruserid == receiverId) ||
             (m.Senderuserid == receiverId && m.Receiveruserid == senderId))
            && m.Messageid > lastMessageId
        )
        .OrderBy(m => m.Createdat)
        .ToListAsync();
        }

        public async Task MarkAllMessagesAsRead(int senderId, int receiverId)
        {
            // We mark them as read only if you are the receiver that's why the compare logic is flipped
            await _context.Chatmessages
                   .Where(c => c.Senderuserid == receiverId &&
                   c.Receiveruserid == senderId &&
                   c.Isread == false)
                   .ExecuteUpdateAsync(c => c.SetProperty(x => x.Isread, true));
        }


    }
}
