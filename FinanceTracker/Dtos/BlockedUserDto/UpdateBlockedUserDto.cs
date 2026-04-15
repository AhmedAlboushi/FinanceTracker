using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.BlockedUserDto
{
    public class UpdateBlockedUserDto
    {

        [Required]

        public int TargetUserId { get; set; }
    }
}
