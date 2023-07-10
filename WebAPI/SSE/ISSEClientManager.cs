using Shared_Resources.Models.SSE;
using WebAPI.Utils;

namespace WebAPI.SSE
{
    public interface ISSEClientManager<T> where T : ISSESubscriber
    {
        Task PushDataToPlayer(T playerId, SSEData data);
        Task Subscribe(T playerId, HttpResponse response);
        Task Unsubscribe(T player);
    }
}