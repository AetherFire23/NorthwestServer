using Newtonsoft.Json;
using WebAPI.Db_Models;
using WebAPI.Enums;
using WebAPI.Interfaces;

namespace WebAPI.Game_Actions
{
    public class GameActionsRepository : IGameActionsRepository
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;

        public GameActionsRepository(PlayerContext playerContext, IPlayerRepository playerRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
        }

        public void ChangeRoomAction(Player player, Room from, Room to)
        {
            var roomChangeInfo = new RoomChangeInfo()
            {
                Player = player,
                Room1 = from,
                Room2 = to,
            };

            var gameAction = roomChangeInfo.ToGameAction();
            var roomLog = roomChangeInfo.ToRoomLog();
            var notifications = GetNotificationsToPlayersInRoom(gameAction.Id, to.Id, player.Id);

            _playerContext.GameActions.Add(gameAction);
            _playerContext.RoomLogs.Add(roomLog);
            _playerContext.TriggerNotifications.AddRange(notifications);

            _playerContext.SaveChanges();
        }

        private IEnumerable<TriggerNotification> GetNotificationsToPlayersInRoom(Guid gameActionId, Guid newRoomId, Guid playerId)
        {
            var playersInNewRoom = _playerContext.Players.Where(player => player.CurrentGameRoomId == newRoomId && player.Id != playerId).ToList();

            var notifications = playersInNewRoom.Select(p => new TriggerNotification()
            {
                PlayerId = p.Id,
                GameActionId = gameActionId,
                NotificationType = NotificationType.DoorOpen,
                Created = DateTime.Now
            });

            return notifications;
        }
    }
}
