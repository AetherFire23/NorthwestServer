using Newtonsoft.Json;
using WebAPI.Db_Models;
using WebAPI.Entities;
using WebAPI.Enums;
using WebAPI.GameTasks;
using WebAPI.Models;

namespace WebAPI.Dummies
{
    public static class DummyValues
    {
        public static Guid defaultGameGuid = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
        public static Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");
        public static Guid defaultplayer2guid = new Guid("b3543b2e-cd81-479f-b99e-d11a8aab37a0");

        public static Player Ben = new Player()
        {
            ActionPoints = 4,
            CurrentChatRoomId = defaultGameGuid,
            CurrentGameRoomId = Guid.Empty, // must be set 
            GameId = defaultGameGuid,
            HealthPoints = 5,
            Id = defaultplayer2guid,
            Name = "Ben",
            Profession = Enums.RoleType.Commander,
            X = -15,
            Y = -5,
            Z = 0,
        };

        public static Player Fred = new Player()
        {
            ActionPoints = 4,
            CurrentChatRoomId = defaultGameGuid,
            CurrentGameRoomId = Guid.Empty,
            GameId = defaultGameGuid,
            HealthPoints = 5,
            Id = defaultPlayer1Guid,
            Name = "Fred",
            Profession = Enums.RoleType.Commander,
            X = -15,
            Y = -5,
            Z = 0,
        };

        public static Station Station = new Station()
        {
            Id = Guid.NewGuid(),
            Name = "CookStation1", // pour faire la diff ds unity, va avoir besoin de template hehelol
            GameTaskCode = GameTaskCode.Cook,
            GameId = defaultGameGuid,
            SerializedProperties = JsonConvert.SerializeObject(new CookStationProperties()
            {
                MoneyMade = 5,
                State = State.Pristine
            }),
        };

        public static Item Item = new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Wrench,
            OwnerId = defaultPlayer1Guid,
        };
        public static Game Game = new Game()
        {
            Id = defaultGameGuid,
            Active = true,
            NextTick = DateTime.UtcNow.AddSeconds(5),
        };
    }
}
