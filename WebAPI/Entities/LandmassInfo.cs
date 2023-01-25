using WebAPI.Db_Models;

namespace WebAPI.Entities
{
    public class LandmassInfo
    {
        // public LandmasLayout Layout { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Station> Stations { get; set; }
    }
}
