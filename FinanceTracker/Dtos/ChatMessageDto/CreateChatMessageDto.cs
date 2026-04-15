using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.ChatMessageDto
{
    public class CreateChatMessageDto
    {


        [Required]
        public string Content { get; set; } = null!;

    }
}
