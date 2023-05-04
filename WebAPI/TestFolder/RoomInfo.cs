
using LandmassTests;

namespace WebAPI.TestFolder
{
    public class RoomInfo
    {
        public string Name { get; set; } = string.Empty;

        public Dictionary<Direction, string> Neighbors { get; set; } = new Dictionary<Direction, string>();

    }
}
