using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        public int ReferenceId { get; set; } // Pour l'enum
        public Guid Owner { get; set; } // pourrait etre un joueur ou une room
    }
}
