using WebAPI.Enums;

namespace WebAPI.TestFolder
{
    public class RoomInfo
    {
        public string Name { get; set; }

        public Dictionary<Direction, string> Neighbors { get; set; }

    }
}
