namespace WebAPI.GameState_Management
{
    public class PlayersInChatRoom
    {
        public Guid RoomId { get; set; }
        public List<Player> Players { get; set; } = new List<Player>(); 
    }
}
