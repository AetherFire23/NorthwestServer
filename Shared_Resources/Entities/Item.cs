﻿using Shared_Resources.Entities;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using System.ComponentModel.DataAnnotations;
using Shared_Resources.Interfaces;

namespace Shared_Resources.Entities
{
    public class Item : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; } // pourrait etre un joueur ou une room
        public ItemType ItemType { get; set; } // Pour l'enum
    }
}