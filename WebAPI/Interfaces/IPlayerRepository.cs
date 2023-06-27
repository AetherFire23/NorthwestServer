using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;

public interface IPlayerRepository
{
    public Player GetPlayer(string name);
    public Task<Item> GetItemAsync(Guid itemId);
    public Task<Player> GetPlayerAsync(Guid id);
    public Task<List<PrivateInvitation>> GetPlayerInvitations(Guid playerId);

    //public List<Player> GetPlayersInGame(Guid gameId);
    //public PlayerDTO GetPlayerDTO(Guid playerId);

    // public RoomDTO GetRoomDTO(Guid roomId);

    public Task<PlayerDTO> MapPlayerDTOAsync(Guid playerId);
    public Task<List<TriggerNotificationDTO>> GetTriggerNotificationsAsync(Guid playerId, DateTime? timeStamp);
    public List<TriggerNotification> GetAllTriggersOfType(Guid playerId, NotificationType notificationType);
    Task<List<Log>> GetAccessibleLogs(Guid playerId, Guid gameId, DateTime? lastTimeStamp);
    Task<List<Player>> GetPlayersInGameAsync(Guid gameId);
} 

