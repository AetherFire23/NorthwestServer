using Shared_Resources.Interfaces;
using System;

namespace Shared_Resources.Entities
{
    public class Message : IEntity
    {
        //[Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Guid GameId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime? Created { get; set; }
    }
}