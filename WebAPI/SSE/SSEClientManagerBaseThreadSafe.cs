using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Utils;
using WebAPI.Utils.Extensions;

namespace WebAPI.SSE
{
    public abstract class SSEClientManagerBaseThreadSafe<T> : IClientManager
        where T : ISSESubscriber
    {
        // this might lead to thread starvation becaues Task.Run is called
        // consider just calling everything from single thread.
        protected ConcurrentDictionary<T, ClientQueueProcessor> SubscribedPlayers { get; }
            = new ConcurrentDictionary<T, ClientQueueProcessor>(new ISSESubscriberKeyComparer<T>());

        public SSEClientManagerBaseThreadSafe()
        {

        }

        public async Task DispatchSSEData()
        {

        }

        public async Task PushDataToPlayer(T playerId, SSEData data)
        {
            var queueProcessor = SubscribedPlayers[playerId];
            await queueProcessor.HttpResponse.WriteAndFlushSSEData(data);
        }

        public async Task Subscribe(T playerId, HttpResponse response)
        {
            ClientQueueProcessor singleProcessor = new ClientQueueProcessor(response);
            SubscribedPlayers.TryAdd(playerId, singleProcessor);
        }

        public async Task Unsubscribe(T player)
        {
            var pair = SubscribedPlayers.First(x => x.Key.Id == player.Id);
            SubscribedPlayers.TryRemove(pair);
        }
    }
}
