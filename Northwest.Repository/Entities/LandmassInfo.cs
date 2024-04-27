namespace Northwest.Persistence.Entities;

public class LandmassInfo
{
    // public LandmasLayout Layout { get; set; }
    public List<Room> Rooms { get; set; } = new List<Room>();
    public List<Station> Stations { get; set; } = new List<Station>();
}
