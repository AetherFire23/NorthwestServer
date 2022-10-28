using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; }

        public RoomType RoomType { get; set; }

        
    }
}
