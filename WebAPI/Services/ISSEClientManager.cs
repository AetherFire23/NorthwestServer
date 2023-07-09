using Shared_Resources.Models.SSE;
using WebAPI.Utils;

namespace WebAPI.Services
{
    public interface ISSEClientManager
    {
        Task PushDataToAllPlayersInGame<T>(Guid gameId, SSEData<T> data);

        Task PushDataIfOnline<T>(PlayerStruct playerId, SSEData<T> data);

        Task PushDataToPlayer<T>(PlayerStruct playerId, SSEData<T> data);

        Task Subscribe(PlayerStruct playerId, HttpResponse response);

        Task Unsubscribe(PlayerStruct playerId);
        Task PushDataToPlayer<T>(Guid playerId, SSEData<T> data);
        List<Guid> GetLoggedPlayers(Guid gameId);
    }
}