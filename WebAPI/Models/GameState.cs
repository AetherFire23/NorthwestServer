using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Models
{
    public class GameState
    {
        public PlayerDTO PlayerDTO { get; set; }
        public RoomDTO Room { get; set; }
        public List<Message> NewMessages { get; set; }
        public List<Player> Players { get; set; }
        public List<TriggerNotificationDTO> TriggerNotifications { get; set; }
        public List<PrivateChatRoomParticipant> PrivateChatRooms { get; set; }
        public List<Log> Logs { get; set; }
        public List<RoomDTO> Rooms { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string SerializedLayout { get; set; }
    }
}