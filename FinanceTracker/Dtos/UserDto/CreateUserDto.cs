using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.UserDto
{
    public class CreateUserDto
    {

        [Required]
        [StringLength(50)]

        public string Username { get; set; } = null!;

        [Required]
        [StringLength(254)]
        [EmailAddress]

        public string Email { get; set; } = null!;

        [Required]
        [StringLength(40)]

        public string Password { get; set; } = null!;




    }
}
