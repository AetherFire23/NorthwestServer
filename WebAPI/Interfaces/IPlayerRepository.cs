
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using WebAPI;

public interface IPlayerRepository
{
    public Player GetPlayer(string name);
    public Item GetItem(Guid itemId);
    public Player GetPlayer(Guid id);
    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId);
    public List<Player> GetPlayersInGame(Guid gameId);
    //public PlayerDTO GetPlayerDTO(Guid playerId);

   // public RoomDTO GetRoomDTO(Guid roomId);

    public PlayerDTO MapPlayerDTO(Guid playerId);
    public List<TriggerNotificationDTO> GetTriggerNotifications(Guid playerId, DateTime? timeStamp);
    public List<TriggerNotification> GetAllTriggersOfType(Guid playerId, NotificationType notificationType);
    public List<Log> GetAccessibleLogs(Guid playerId, DateTime? lastTimeStamp);
} 

