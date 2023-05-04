using Shared_Resources.Interfaces;
using System;
namespace Shared_Resources.Entities
{
    public class AdjacentRoom : IEntity
    {
        //[Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid RoomId { get; set; }
        public Guid AdjacentId { get; set; }
        public bool IsLandmassConnection { get; set; }
    }
}
