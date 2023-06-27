﻿using System;
using Shared_Resources.Interfaces;

namespace Shared_Resources.Entities
{
    public class Message : IEntity
    {
        //[Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SenderName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public Guid GameId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
    }
}