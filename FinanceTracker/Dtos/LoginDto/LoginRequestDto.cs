using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.LoginDto
{
    public class LoginRequestDto
    {
        [EmailAddress]
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }


        [Required]
        [MinLength(6)]
        [MaxLength(40)]
        public string Password { get; set; }
    }
}
