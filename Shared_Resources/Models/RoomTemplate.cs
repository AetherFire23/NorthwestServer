namespace Shared_Resources.Models
{
    public class RoomTemplate // Devra etre divise en Room, adjacentRoom, Item, Player, etc. Pour pouvoir initialiser le player dans la bonne room. 
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; }

        // What differentiates it from the Entity.
        public List<string> AdjacentNames { get; set; }
        public RoomType RoomType { get; set; }
    }
}
