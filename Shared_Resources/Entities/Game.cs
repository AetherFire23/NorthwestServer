using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } 
        public DateTime NextTick { get; set; }
        public bool Active { get; set; }
    }
}
