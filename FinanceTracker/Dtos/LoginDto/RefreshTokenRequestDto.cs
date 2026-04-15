using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.LoginDto
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]

        public string RefreshToken { get; set; }
    }
}
