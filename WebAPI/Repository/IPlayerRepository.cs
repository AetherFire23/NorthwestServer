
using WebAPI;
using WebAPI.Game_Actions;
using WebAPI.Models;
using WebAPI.Models.DTOs;

public interface IPlayerRepository
{

    public Player GetPlayer(string name);

    public Player GetPlayer(Guid id);
    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId);
    public List<Player> GetPlayersInCurrentGame(Guid gameId);
    //public PlayerDTO GetPlayerDTO(Guid playerId);

    public RoomDTO GetRoomDTO(Guid roomId);

    public PlayerDTO MapPlayerDTO(Guid playerId);
    public List<TriggerNotificationDTO> GetTriggerNotifications(Guid playerId, DateTime? timeStamp);
}

