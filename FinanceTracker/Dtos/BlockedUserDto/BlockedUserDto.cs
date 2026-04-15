namespace FinanceTracker.Dtos.BlockedUserDto
{
    public class BlockedUserDto
    {

        public int UserId { get; set; }

        public string TargetUsername { get; set; }
        public int TargetUserId { get; set; }
        public DateOnly CreatedAt { get; set; }

    }
}
