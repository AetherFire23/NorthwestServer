using Shared_Resources.Entities;

namespace WebAPI.Services
{
    public interface ISSEManager
    {
        Task SendItemChangedOwnerEvent(Guid gameId);
        Task SendNewLogEvent(Log log);
    }
}