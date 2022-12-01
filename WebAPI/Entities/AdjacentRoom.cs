using System.ComponentModel.DataAnnotations;

namespace WebAPI.Db_Models
{
    public class AdjacentRoom
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid AdjacentId { get; set; }
    }
}
