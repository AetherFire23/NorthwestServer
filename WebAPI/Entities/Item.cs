using WebAPI.Enums;
using WebAPI.Interfaces;
namespace WebAPI.Entities;

public class Item : IEntity
{
    //[Key]
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; } // pourrait etre un joueur ou une room
    public ItemType ItemType { get; set; } // Pour l'enum
}
