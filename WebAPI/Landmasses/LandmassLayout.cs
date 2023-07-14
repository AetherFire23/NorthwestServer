using LandmassTests;
using System.Text.Json.Serialization;

namespace WebAPI.TestFolder;

public class LandmassLayout
{
    // principes :
    // les room nont pas de ID direction ou de whatever
    // techniquement, ds unity, jai besoin de placer une door dans la bonne direction
    // ET la room dans la bonne direction
    // niveau du gameplay, fait pas gr chose davoir la direction
    // so ce nest pas dans les room.


    public List<List<RoomCell>> Cells { get; set; }
    [JsonIgnore]
    public List<RoomCell> AllCels => Cells.SelectMany(x => x).ToList();

    /// <summary>
    /// Rooms = celles que le string != string.Empty
    /// </summary>
    [JsonIgnore]
    public List<RoomCell> AllRooms => AllCels.Where(x => x.Name != "").ToList();

    [JsonIgnore]
    public List<RoomCell> FirstColumnCells => Cells[0];

    public DateTime AddStamp { get; set; }

    public int Width;
    public int Height;
}
