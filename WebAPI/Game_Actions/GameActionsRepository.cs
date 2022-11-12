using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Game_Actions
{
    public class GameActionsRepository : IGameActionsRepository
    {
        PlayerContext _playerContext;
        public GameActionsRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public void ChangeRoomAction(Player p, Room from, Room to)
        {
            var roomChangeInfo = new RoomChangeInfo()
            {
                Player = p,
                Room1 = from,
                Room2 = to,
            };
            string serializedRoomChangeInfo = JsonConvert.SerializeObject(roomChangeInfo);
            var gameAction = new GameAction()
            {
                Id = Guid.NewGuid(),
                GameActionType = Enums.GameActionType.RoomChanged,
                GameId = p.Id,
                TimeStamp = DateTime.Now,
                SerializedProperties = serializedRoomChangeInfo,
            };

            var roomLog = new RoomLog()
            {
                Id = Guid.NewGuid(),
                TriggeringPlayerId = p.Id,
                PrivacyLevel = Enums.RoomLogPrivacyLevel.Public,
                EventText = $"Player {p.Name}, has changed room from {from.Name}, to {to.Name}]",
                TimeStamp = gameAction.TimeStamp,
            };

            var trigNotif = new TriggerNotification()
            {
                NotificationType = Enums.NotificationType.DoorOpen,
                SerializedProperties = serializedRoomChangeInfo,
            };

            _playerContext.GameActions.Add(gameAction);
            _playerContext.RoomLogs.Add(roomLog);
            _playerContext.TriggerNotifications.Add(trigNotif);
            _playerContext.SaveChanges();
        }
    }
}
