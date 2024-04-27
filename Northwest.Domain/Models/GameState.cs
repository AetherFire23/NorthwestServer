using Northwest.Domain.Dtos;
using Northwest.Domain.GameTasks;
using Northwest.Persistence.Entities;
using System.ComponentModel.DataAnnotations;
namespace Northwest.Domain.Models;


public class GameState
{
    [Required]
    public PlayerDto PlayerDto { get; set; } = new PlayerDto();
    // public RoomDTO Room { get; set; } = new RoomDTO(); // ark va etre called a eter delted
    //public RoomDTO LocalPlayerRoom => Rooms.FirstOrDefault(x => x.Id == PlayerDTO.CurrentGameRoomId);

    [Required]
    public RoomDto LocalPlayerRoom
    {
        get
        {
            RoomDto? room = Rooms.FirstOrDefault(x => x.Id == PlayerDto.CurrentGameRoomId);
            if (room is null)
            {
                Console.Out.WriteLine("LocalPlayerRoom was null, make sure this is intended");
                return null;
            }
            return room;
        }
    }

    [Required]
    public List<Message> NewMessages { get; set; } = [];

    [Required]
    public List<Player> Players { get; set; } = [];

    [Required]
    public List<PrivateChatRoomParticipant> PrivateChatRoomParticipants { get; set; } = [];

    [Required]
    public List<Log> Logs { get; set; } = [];

    [Required]
    public List<RoomDto> Rooms { get; set; } = [];
    public DateTime? TimeStamp { get; set; }
    public string SerializedLayout { get; set; } = string.Empty;
    public List<PrivateChatRoom> PrivateChatRooms { get; set; } = [];
    public List<Station> Stations { get; set; } = [];

    [Required]
    public List<GameTaskAvailabilityResult> VisibleGameTasks { get; set; } = [];
    public Guid GameId => PlayerDto.GameId;
    public Guid PlayerUID => PlayerDto.Id;
}