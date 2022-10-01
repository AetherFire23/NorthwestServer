using WebAPI.Models;

namespace WebAPI.GameState_Management.Game_State_Repository
{
    public interface IGameStateRepository
    {
        public GameState GetPlayerGameState(Guid playerId, DateTime? lastTimeStamp);
        public List<Message> GetNewMessages(DateTime? lastTimeStamp, Guid gameId);
        public List<Message> GetMessagesFromGameId(Guid gameId);
        public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId);
    }
}
