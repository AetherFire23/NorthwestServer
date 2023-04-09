using Shared_Resources.Interfaces;
using System;

namespace Shared_Resources.Entities
{
    public class Log : IEntity
    {
        //[Key]
        public Guid Id { get; set; }
        public Guid TriggeringPlayerId { get; set; } // for private logs
        public Guid RoomId { get; set; }
        public bool IsPublic { get; set; }
        public string EventText { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
