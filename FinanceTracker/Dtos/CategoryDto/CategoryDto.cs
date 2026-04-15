namespace FinanceTracker.Dtos.CategoryDto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;


        public string ColorHex { get; set; } = null!;

        public string? IconName { get; set; }

    }
}
