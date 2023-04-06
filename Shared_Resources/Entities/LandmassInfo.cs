using Shared_Resources.Db_Models;

namespace Shared_Resources.Entities
{
    public class LandmassInfo
    {
        // public LandmasLayout Layout { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Station> Stations { get; set; }
    }
}
