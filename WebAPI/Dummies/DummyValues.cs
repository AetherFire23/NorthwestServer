using Newtonsoft.Json;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Enums;
using WebAPI.Models;
namespace WebAPI.Dummies;

public static class DummyValues
{
    public static Guid defaultGameGuid = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
    public static Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");
    public static Guid defaultplayer2guid = new Guid("b3543b2e-cd81-479f-b99e-d11a8aab37a0");
    public static Guid defaultPrivateChatRoomId = new Guid("fedfdb8cc0634d319e6e21cdf3d0790a");


    public static List<Player> PlayerList = new List<Player>()
    {
        Ben,
        Fred,
    };

    public static UserDto FredUser { get; set; } = new UserDto()
    {
        Id = defaultPlayer1Guid,
        Name = "RealFred",
    };

    public static UserDto BenUser { get; set; } = new UserDto()
    {
        Id = defaultplayer2guid,
        Name = "RealBen",
    };

    public static Player Ben = new Player()
    {
        ActionPoints = 4,
        CurrentGameRoomId = Guid.Empty, // must be set 
        UserId = FredUser.Id,
        GameId = defaultGameGuid,
        HealthPoints = 5,
        Id = defaultplayer2guid,
        Name = "Ben",
        Profession = RoleType.Commander,
        X = -15,
        Y = -5,
        Z = 0,
    };

    public static Player Fred = new Player()
    {
        ActionPoints = 4,
        CurrentGameRoomId = Guid.Empty,
        GameId = defaultGameGuid,
        HealthPoints = 5,
        Id = defaultPlayer1Guid,
        Name = "Fred",
        Profession = RoleType.Commander,
        X = -15,
        Y = -5,
        Z = 0,
    };

    public static Station Station = new Station()
    {
        Id = Guid.NewGuid(),
        Name = "CookStation1", // pour faire la diff ds unity, va avoir besoin de template hehelol
        GameId = defaultGameGuid,
        SerializedProperties = JsonConvert.SerializeObject(new CookStationProperties()
        {
            MoneyMade = 5,
            State = State.Pristine
        }),
    };

    public static TriggerNotification TriggerNotification1 = new TriggerNotification()
    {
        Id = Guid.NewGuid(),
        Created = DateTime.UtcNow,
        GameActionId = Guid.NewGuid(),
        IsReceived = false,
        NotificationType = NotificationType.CycleChanged,
        PlayerId = Guid.Empty,
        SerializedProperties = string.Empty,
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
        IsActive = true,
        NextTick = DateTime.UtcNow.AddSeconds(5),
    };

    public static Expedition Expedition1 = new Expedition()
    {
        Name = "Expedition1",
        GameId = defaultGameGuid,
        Id = new Guid("5e77ae8e-5d92-42d9-836d-2ef8720a12f7"),
        IsAvailableForCreation = true,
        IsCreated = false,
        LeaderId = Guid.Empty,
    };

    public static Message Message1 = new Message()
    {
        Id = Guid.NewGuid(),
        Created = DateTime.UtcNow,
        GameId = defaultGameGuid,
        SenderName = "The Master",
        RoomId = defaultGameGuid,
        Text = "I am god"
    };

    public static Message Message2 = new Message()
    {
        Id = Guid.NewGuid(),
        Created = DateTime.UtcNow,
        GameId = defaultGameGuid,
        SenderName = "The Master",
        RoomId = defaultPrivateChatRoomId,
        Text = "I am in private room 2"
    };

    public static PrivateChatRoom PrivateChatRoom = new PrivateChatRoom()
    {
        Id = defaultPrivateChatRoomId,
        ChatRoomName = "MyRoomName"
    };

    public static PrivateChatRoomParticipant PrivateChatRoomParticipant = new PrivateChatRoomParticipant()
    {
        Id = Guid.NewGuid(),
        ParticipantId = defaultPlayer1Guid,
        RoomId = defaultPrivateChatRoomId
    };
    public static Item Freditem = new Item()
    {
        Id = Guid.NewGuid(),
        ItemType = ItemType.Wrench,
        OwnerId = defaultPlayer1Guid
    };
    public static Item GetRandomItem(Guid ownerId)
    {
        return new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Wrench,
            OwnerId = ownerId
        };
    }

    public static Log SomeLog(Guid roomId, Guid gameId)
    {
        Log log = new Log()
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            Created = DateTime.UtcNow,
            CreatedBy = "MASterLolz",
            EventText = "JE suis un lolzida",
            IsPublic = true,
            RoomId = roomId,
            TriggeringPlayerId = defaultPlayer1Guid,
        };
        return log;
    }


    public static User MyUser = new User()
    {
        Id = Guid.NewGuid(),
        Name = "Fred",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("mereEnShorts"),
    };

    public static Role PereNoel = new Role()
    {
        Id = new Guid("4e9ec37d-71d9-4e66-8970-45424db1eeb1"),
        RoleName = RoleName.PereNoel,
    };

    public static UserRole MyUserRole = new UserRole()
    {
        Id = Guid.NewGuid(),
        Role = PereNoel,
        User = MyUser,
    };
}