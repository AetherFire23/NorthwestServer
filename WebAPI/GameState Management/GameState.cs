using WebAPI.Models;

namespace WebAPI.GameState_Management
{
    public class GameState // est un objet construit, pas dans la database
    {
        public List<Message> NewMessages { get; set; }
        public List<Player> Players { get; set; }
        public List<PrivateInvitation> Invitations { get; set; }
        public List<PrivateChatRoomParticipant> PrivateChatRooms { get; set; }
        public Player Player { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
