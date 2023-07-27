using Shared_Resources.Models.SSE;

namespace WebAPI.SSE.SSECore;

public interface ISSEClientManager<T> where T : ISSESubscriber
{
    Task PushDataToPlayer(T playerId, SSEData data);
    Task Subscribe(T playerId, HttpResponse response);
    Task Unsubscribe(T player);
}