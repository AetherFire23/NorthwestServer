
using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces;

public interface IGameStateRepository
{
    public Task<List<Message>> GetNewMessagesAsync(DateTime? lastTimeStamp, Guid gameId);
    public List<Message> GetMessagesFromGameId(Guid gameId);
    public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId);
    Task<ClientCallResult> GetPlayerGameStateAsync(Guid playerId, DateTime? lastTimeStamp);
}
