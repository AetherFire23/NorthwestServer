using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Utils.Extensions;

namespace WebAPI.SSE
{
    public class SSEClientManagerBase<T> : IClientManager where T : ISSESubscriber
    {
        protected ConcurrentDictionary<T, HttpResponse> SubscriberResponseMap { get; }
           = new ConcurrentDictionary<T, HttpResponse>(new ISSESubscriberKeyComparer<T>());

        protected List<T> Subscribers => SubscriberResponseMap.Keys.ToList();

        public SSEClientManagerBase()
        {

        }

        public async Task PushDataToSubscriber(T subscriber, SSEData data)
        {
            var response = SubscriberResponseMap[subscriber];
            await response.WriteAndFlushSSEData(data);
        }

        public async Task Subscribe(T subscriber, HttpResponse response)
        {
            SubscriberResponseMap.TryAdd(subscriber, response);
        }

        public async Task Unsubscribe(T subscriber)
        {
            var pair = SubscriberResponseMap.First(x => x.Key.Id == subscriber.Id);
            SubscriberResponseMap.TryRemove(pair);
        }
    }
}
