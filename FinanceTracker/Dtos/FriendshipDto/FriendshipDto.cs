namespace FinanceTracker.Dtos.FriendshipDto
{
    public class FriendshipDto
    {
        public int FriendshipId { get; set; }


        public int FriendUserId { get; set; }

        public string FriendUsername { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
