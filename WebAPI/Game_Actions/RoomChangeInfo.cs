using WebAPI.Models;

namespace WebAPI.Game_Actions
{
    public class RoomChangeInfo
    {
        public Player Player { get; set; }
        public Room Room1 { get; set; }
        public Room Room2 { get; set; }
    }
}
