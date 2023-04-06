using Shared_Resources.Entities;
namespace Shared_Resources.DTOs
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; }
        public List<Player> Players { get; set; }
        public RoomType RoomType { get; set; }
    }
}
