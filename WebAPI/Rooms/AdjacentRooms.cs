namespace WebAPI.Rooms
{
    public static class AdjacentRooms
    {
        public static Dictionary<RoomType, List<RoomType>> dic = new Dictionary<RoomType, List<RoomType>>()
        {
            { RoomType.Start, new List<RoomType>() {RoomType.Second} },
            { RoomType.Second, new List<RoomType>() {RoomType.Start } },
        };
    }
}