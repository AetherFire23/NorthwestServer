using Northwest.Persistence.Enums;
namespace Northwest.Persistence.Entities;

public class Item : EntityBase
{
    public Guid OwnerId { get; set; } // pourrait etre un joueur ou une room
    public ItemType ItemType { get; set; } // Pour l'enum
}
