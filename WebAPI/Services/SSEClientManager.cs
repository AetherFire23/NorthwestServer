using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Utils;

namespace WebAPI.Services
{
    public class SSEClientManager : ISSEClientManager
    {
        private ConcurrentDictionary<PlayerStruct, HttpResponse> _subscribedPlayers { get; } = new(new PlayerKeyComparer());
        private ConcurrentDictionary<ISSESubscriber, (HttpResponse Response, ConcurrentQueue<SSEData> Data)>
        // 4 AM]TeBeClone: yeah just don't do that
        //[8:34 AM] TeBeClone: write to a queue that support concurrency
        //[8:34 AM] TeBeClone: and using only a single thread to read the queue and pop item + write to stream properly
        //[8:35 AM]TeBeClone: since SSE is CRLF based for property and 2 CRLF based on new message you need to make sure 1 item of the queue it a full item on the SSE part

        public List<Guid> GetLoggedPlayers(Guid gameId)
        {
            List<Guid> playerIds = _subscribedPlayers.Keys.Where(x => x.GameId == gameId)
                .Select(x => x.Id).ToList();
            return playerIds;
        }

        public async Task PushDataToAllPlayersInGame<T>(Guid gameId, SSEData<T> data)
        {
            var allPlayers = _subscribedPlayers.Keys.Where(x => x.GameId == gameId).ToList();

            foreach (PlayerStruct playerStruct in allPlayers)
            {
                await PushDataIfOnline(playerStruct, data);
            }
        }

        public async Task PushDataIfOnline<T>(PlayerStruct playerId, SSEData<T> data)
        {
            bool isOnline = _subscribedPlayers.ContainsKey(playerId);
            if (!isOnline) return;

            await PushDataToPlayer(playerId, data);
        }

        public async Task PushDataToPlayer<T>(PlayerStruct playerId, SSEData<T> data)
        {
            var response = _subscribedPlayers[playerId];
            await response.WriteAndFlushSSEData(data);
        }

        public async Task PushDataToPlayer<T>(Guid playerId, SSEData<T> data)
        {
            PlayerStruct playerStruct = new PlayerStruct(playerId);

            var response = _subscribedPlayers[playerStruct];
            await response.WriteAndFlushSSEData(data);
            Console.WriteLine($"sent {data.EventType} to {playerId}");
        }

        public async Task Subscribe(PlayerStruct playerId, HttpResponse response)
        {
            _subscribedPlayers.TryAdd(playerId, response);
        }

        public async Task Unsubscribe(PlayerStruct player)
        {
            var pair = _subscribedPlayers.First(x => x.Key.Id == player.Id);
            _subscribedPlayers.TryRemove(pair);
        }
    }

    public static class HttpResponseExtensions
    {
        public static async Task WriteAndFlushSSEData<T>(this HttpResponse httpResponse, SSEData<T> data)
        {
            // introdure le lock ? y se passe quoi si 2-3 methods writent en meme temps ? 
            string parseableLine = data.ConvertToReadableLine();
            await httpResponse.WriteAsync(parseableLine);
            await httpResponse.Body.FlushAsync();
        }
    }
}
