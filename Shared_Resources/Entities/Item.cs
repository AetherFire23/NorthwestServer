using Shared_Resources.Entities;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
namespace Shared_Resources.Entities
{
    public class Item : IEntity
    {
        //[Key]
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; } // pourrait etre un joueur ou une room
        public ItemType ItemType { get; set; } // Pour l'enum
    }
}
