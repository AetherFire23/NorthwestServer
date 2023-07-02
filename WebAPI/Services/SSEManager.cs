using Shared_Resources.Entities;
using System.Collections.Concurrent;

namespace WebAPI.Services
{
    public class SSEManager : ISSEManager
    {
        public ConcurrentBag<User> Subscribed = new();

        public async Task Subscribe(User user)
        {
            Subscribed.Add(user);
        }

        public async Task Unsubscribe(User user)
        {
            Subscribed.TryTake(out var subscribedUser);
            Console.WriteLine(subscribedUser);
        }

      //  public async Task 
    }
}
