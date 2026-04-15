using FinanceTracker.IService;
using System.Collections.Concurrent;

namespace FinanceTracker.Service
{
    public class ActiveUserTrackerService : IActiveUserTrackerService
    {






        private readonly ConcurrentDictionary<int, DateTime> _users = new();

        public void Track(int userId)
        {

            _users[userId] = DateTime.UtcNow.AddMinutes(30);



        }

        public int GetActiveUsers() => _users.Count;

        public void ClearStale(TimeSpan threshold)

        {
            var cutoff = DateTime.UtcNow - threshold;
            foreach (var user in _users)
            {
                if (user.Value < cutoff)
                {
                    _users.TryRemove(user.Key, out _);
                }
            }


        }

    }
}
