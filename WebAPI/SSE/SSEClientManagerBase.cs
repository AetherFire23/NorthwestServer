using Azure;
using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Constants;
using WebAPI.Utils;
using WebAPI.Utils.Extensions;

namespace WebAPI.SSE
{
    public abstract class SSEClientManagerBase<T> : IClientManager where T : ISSESubscriber
    {
        protected ConcurrentDictionary<T, HttpResponse> SubscriberResponseMap { get; }
           = new ConcurrentDictionary<T, HttpResponse>(new ISSESubscriberKeyComparer<T>());

        public List<T> Subscribers => SubscriberResponseMap.Keys.ToList();

        public SSEClientManagerBase()
        {

        }

        public async Task InitializeAndSubscribe(T subscriber, HttpResponse response)
        {
            Console.WriteLine("req received from client");
            response.Headers.Add("Content-Type", "text/event-stream");
            response.Headers.Add("Cache-Control", "no-cache");
            response.Headers.Add("Connection", "keep-alive");
            try
            {
                // send heartbeat to client to ensure successCode or else it just hangs 
                await response.WriteAndFlushSSEData(Heart.Beat);
                await Subscribe(subscriber, response);

                await Task.Delay(Timeout.Infinite, response.HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                await Unsubscribe(subscriber);
            }
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
