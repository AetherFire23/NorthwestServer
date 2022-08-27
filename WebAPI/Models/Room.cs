using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }

        public Guid GameId { get; set; }
        
        public int ReferenceId { get; set; } // pour l'enum
    }
}

public enum ItemType
{
    Wrench,
    Hose
}