namespace FinanceTracker.Dtos.IncomeDto
{
    public class IncomeDto
    {
        public int IncomeId { get; set; }

        public decimal Amount { get; set; }

        public string Username { get; set; }

        public string IncomeSource { get; set; }
        public string? Description { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
