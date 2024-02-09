using Newtonsoft.Json;
using System.Drawing;

namespace WebAPI.Landmasses;

public class RoomCell
{
    public string Name { get; set; }

    public Dictionary<Direction, RoomCell> Neighbors { get; set; } = new();

    public Dictionary<Direction, RoomCell> DoorConnections { get; set; } = new();

    [JsonIgnore]
    public Dictionary<Direction, RoomCell> RoomNeighbors => Neighbors.Where(x => x.Value.Name != "").ToDictionary(x => x.Key, x => x.Value);

    [JsonIgnore]
    public Dictionary<Direction, RoomCell> EmptyNeighbors => Neighbors.Where(x => x.Value.Name == "").ToDictionary(x => x.Key, x => x.Value);

    public Rectangle FormsRectangle;

    public Point Position;

    public string DebugCreator = string.Empty;

    public bool IsEmptySpace => Name == "";
    public void ConnectNeighbors(RoomCell roomCell, Direction direction)
    {
        Neighbors.Add(direction, roomCell);

        Direction oppositeDirection = DirectionHelper.GetOppositeDirection(direction);
        roomCell.Neighbors.Add(oppositeDirection, this);
    }

    public void ConnectDoors(RoomCell roomCell)
    {
        bool alreadyConnected = DoorConnections.Values.Contains(roomCell)
            || roomCell.DoorConnections.Values.Contains(this);


        bool isSelf = roomCell == this;
        if (alreadyConnected || isSelf)
        {
            return;
        }

        KeyValuePair<Direction, RoomCell> keyPair = Neighbors.FirstOrDefault(x => x.Value == roomCell);
        DoorConnections.Add(keyPair.Key, keyPair.Value);

        Direction opposite = DirectionHelper.GetOppositeDirection(keyPair.Key);
        roomCell.DoorConnections.Add(opposite, this);
    }

    public List<RoomCell> GetAllNeighboringRooms()
    {
        List<RoomCell> neighorRooms = Neighbors.Select(x => x.Value)
            .Where(x => x.Name != "").ToList();

        return neighorRooms;
    }

    public void PrintNeighbor()
    {
        Console.WriteLine($"{Name}");

        foreach (KeyValuePair<Direction, RoomCell> cell in Neighbors)
        {
            Console.WriteLine($"{cell.Key}");
        }
    }

    public override string ToString()
    {
        string name = Name == "" ? "Empty" : Name;
        return name;
    }
}
