namespace WebAPI.Landmasses;

public class RoomInfo
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<Direction, string> Neighbors { get; set; } = new Dictionary<Direction, string>();

}
