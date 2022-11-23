using Newtonsoft.Json;
using WebAPI.Db_Models;
using WebAPI.Enums;

namespace WebAPI.Game_Actions
{
    public class RoomChangeInfo
    {
        public Player Player { get; set; }
        public Room Room1 { get; set; }
        public Room Room2 { get; set; }

        public GameAction ToGameAction()
        {
            return new GameAction()
            {
                Id = Guid.NewGuid(),
                GameId = Player.GameId,
                GameActionType = GameActionType.RoomChanged,
                SerializedProperties = JsonConvert.SerializeObject(this),
                Created = DateTime.Now,
                CreatedBy = Player.Name
            };
        }

        public RoomLog ToRoomLog()
        {
            return new RoomLog()
            {
                Id = Guid.NewGuid(),
                TriggeringPlayerId = Player.Id,
                PrivacyLevel = RoomLogPrivacyLevel.Public,
                EventText = $"Player {Player.Name}, has changed room from {Room1.Name}, to {Room2.Name}]",
                Created = DateTime.Now,
                CreatedBy = Player.Name,
            };
        }
    }
}
