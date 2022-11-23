using WebAPI.Db_Models;
using WebAPI.DTOs;

namespace WebAPI.GameState_Management
{
    public class GameState
    {
        public PlayerDTO PlayerDTO { get; set; }
        public RoomDTO Room { get; set; }

        public List<Message> NewMessages { get; set; }
        public List<Player> Players { get; set; }
        public List<TriggerNotificationDTO> TriggerNotifications { get; set; }
        public List<PrivateChatRoomParticipant> PrivateChatRooms { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
