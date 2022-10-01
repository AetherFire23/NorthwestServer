using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        public RoomType RoomType { get; set; }
        
    }
}
