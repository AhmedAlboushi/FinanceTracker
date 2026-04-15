using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.BlockedUserDto
{
    public class CreateBlockedUserDto
    {



        [Required]

        public int TargetUserId { get; set; }

    }
}
