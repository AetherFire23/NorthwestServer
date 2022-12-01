using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Db_Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; } // pourrait etre un joueur ou une room
        public ItemType ItemType { get; set; } // Pour l'enum
    }
}
