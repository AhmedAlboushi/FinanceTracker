using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.UserDto
{
    public class UpdateUserDto
    {

        [StringLength(50)]
        public string Username { get; set; } = null!;








    }
}
