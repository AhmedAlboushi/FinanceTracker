namespace FinanceTracker.Dtos.ChatMessageDto
{
    public class ChatMessageDto
    {
        public int MessageId { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }
    }
}
