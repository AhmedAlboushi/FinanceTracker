namespace FinanceTracker.Dtos.UserDto
{
    public class UserDto
    {
        public int UserId { get; set; }


        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }





    }
}
