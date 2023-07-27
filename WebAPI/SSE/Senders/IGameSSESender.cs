using Shared_Resources.Entities;

namespace WebAPI.SSE;

public interface IGameSSESender
{
    Task SendItemChangedEvent(Guid gameId);
    Task SendNewLogEvent(Log log);
}