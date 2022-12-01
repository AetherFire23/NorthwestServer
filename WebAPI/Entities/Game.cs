using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } 
        public DateTime NextTick { get; set; }
        public bool Active { get; set; }
    }
}
