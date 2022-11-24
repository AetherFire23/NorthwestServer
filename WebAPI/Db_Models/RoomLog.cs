using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Db_Models
{
    public class RoomLog
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TriggeringPlayerId { get; set; } // for private logs
        public RoomLogPrivacyLevel PrivacyLevel { get; set; }
        public string EventText { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
    }
}
