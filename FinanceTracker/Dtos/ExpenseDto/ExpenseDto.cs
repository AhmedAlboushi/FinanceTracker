namespace FinanceTracker.Dtos.ExpenseDto
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }

        public string Username { get; set; }

        public string Category { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public DateOnly CreatedAt { get; set; }
    }
}
