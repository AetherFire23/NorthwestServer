
using WebAPI;
using WebAPI.Db_Models;
using WebAPI.DTOs;

public interface IPlayerRepository
{

    public Player GetPlayer(string name);

    public Player GetPlayer(Guid id);
    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId);
    public List<Player> GetPlayersInGame(Guid gameId);
    //public PlayerDTO GetPlayerDTO(Guid playerId);

    public RoomDTO GetRoomDTO(Guid roomId);

    public PlayerDTO MapPlayerDTO(Guid playerId);
    public List<TriggerNotificationDTO> GetTriggerNotifications(Guid playerId, DateTime? timeStamp);
}

