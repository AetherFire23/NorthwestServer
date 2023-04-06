using Shared_Resources.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class Room : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsLandmass { get; set; }
        public bool IsActive { get; set; }
    }
}
