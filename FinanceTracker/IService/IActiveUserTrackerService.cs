namespace FinanceTracker.IService
{
    public interface IActiveUserTrackerService
    {
        public void ClearStale(TimeSpan timeSpan);
        public void Track(int userId);
        public int GetActiveUsers();
    }
}
