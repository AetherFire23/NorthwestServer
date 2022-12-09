using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Db_Models
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TriggeringPlayerId { get; set; } // for private logs
        public Guid RoomId { get; set; }
        public bool IsPublic { get; set; }
        public string EventText { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
    }
}
