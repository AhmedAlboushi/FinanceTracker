namespace FinanceTracker.Helpers
{
    public static class PaginationHelper
    {
        public static (int page, int pageSize) Validate(int page, int pageSize)
        {
            return (Math.Max(1, page), Math.Clamp(pageSize, 1, 100));
        }
    }
}
