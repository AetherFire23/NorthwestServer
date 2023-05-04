
using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IGameStateRepository
    {
        public Task<GameState> GetPlayerGameStateAsync(Guid playerId, DateTime? lastTimeStamp);
        public Task<List<Message>> GetNewMessagesAsync(DateTime? lastTimeStamp, Guid gameId);
        public List<Message> GetMessagesFromGameId(Guid gameId);
        public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId);
    }
}
