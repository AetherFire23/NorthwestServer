using Shared_Resources.Entities;

namespace WebAPI.SSE
{
    public interface IGameSSESender
    {
        Task SendItemChangedOwnerEvent(Guid gameId);
        Task SendNewLogEvent(Log log);
    }
}