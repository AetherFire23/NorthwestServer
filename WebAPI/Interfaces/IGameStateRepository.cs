using WebAPI.Db_Models;
using WebAPI.GameState_Management;

namespace WebAPI.Interfaces
{
    public interface IGameStateRepository
    {
        public GameState GetPlayerGameState(Guid playerId, DateTime? lastTimeStamp);
        public List<Message> GetNewMessages(DateTime? lastTimeStamp, Guid gameId);
        public List<Message> GetMessagesFromGameId(Guid gameId);
        public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId);
    }
}
