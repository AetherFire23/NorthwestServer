using Shared_Resources.Interfaces;
using System;

namespace Shared_Resources.Entities
{
    public class Message : IEntity
    {
        //[Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public Guid GameId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime? Created { get; set; }
    }
}