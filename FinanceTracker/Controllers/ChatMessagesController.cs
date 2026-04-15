using FinanceTracker.Dtos.ChatMessageDto;
using FinanceTracker.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("GeneralLimiter")]
    [Authorize]
    public class ChatMessagesController : ControllerBase
    {

        private readonly IChatMessagesService _chatMessagesService;
        public ChatMessagesController(IChatMessagesService chatMessagesService)

        {
            _chatMessagesService = chatMessagesService;
        }

        [HttpGet("{receiverId}")]

        public async Task<IActionResult> GetChatMessagesBySenderIdAndRecieverId(int receiverId,
            int page = 1, int pageSize = 50)
        {
            int senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chatMessagesDto = await _chatMessagesService.GetChatMessagesBySenderIdAndRecieverId(senderId, receiverId,
                page, pageSize);

            return Ok(chatMessagesDto);
        }

        [HttpPost("{receiverUserId}")]

        public async Task<IActionResult> SendChatMessage(int receiverUserId, CreateChatMessageDto chatMessageDto)
        {
            int senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _chatMessagesService.SendChatMessage(senderId, receiverUserId, chatMessageDto);

            return Ok("sent");
        }
        [HttpPut("{receiverId}/mark-all-as-read")]

        public async Task<IActionResult> MarkAllMessagesAsRead(int receiverId)
        {
            int senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _chatMessagesService.MarkAllMessagesAsRead(senderId, receiverId);

            return Ok();
        }

        [HttpGet("{receiverId}/new/{lastMessageId}")]
        public async Task<IActionResult> GetNewMessages(int receiverId, int lastMessageId)
        {
            int senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var newMessagesDto = await _chatMessagesService.GetNewMessages(senderId, receiverId, lastMessageId);

            return Ok(newMessagesDto);

        }
    }
}
