using WebAPI.Entities;
using WebAPI.Enums;

namespace WebAPI.Game_Actions;

public class GameActionsRepository
{
    private readonly PlayerContext _playerContext;
    private readonly PlayerRepository _playerRepository;

    public GameActionsRepository(PlayerContext playerContext, PlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
    }

    public void ChangeRoomAction(Player player, Room from, Room to)
    {
        RoomChangeInfo roomChangeInfo = new RoomChangeInfo()
        {
            Player = player,
            Room1 = from,
            Room2 = to,
        };

        GameAction gameAction = roomChangeInfo.ToGameAction();
        Log roomLog = roomChangeInfo.ToRoomLog();
        _ = GetNotificationsToPlayersInRoom(gameAction.Id, to.Id, player.Id);

        _ = _playerContext.GameActions.Add(gameAction);
        _ = _playerContext.Logs.Add(roomLog);

        _ = _playerContext.SaveChanges();
    }

    private IEnumerable<TriggerNotification> GetNotificationsToPlayersInRoom(Guid gameActionId, Guid newRoomId, Guid playerId)
    {
        List<Player> playersInNewRoom = _playerContext.Players.Where(player => player.CurrentGameRoomId == newRoomId && player.Id != playerId).ToList();

        IEnumerable<TriggerNotification> notifications = playersInNewRoom.Select(p => new TriggerNotification()
        {
            PlayerId = p.Id,
            GameActionId = gameActionId,
            NotificationType = NotificationType.DoorOpen,
            Created = DateTime.Now
        });

        return notifications;
    }
}
