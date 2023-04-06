using Shared_Resources.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Db_Models
{
    public class AdjacentRoom : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid AdjacentId { get; set; }
    }
}
