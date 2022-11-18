using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class TriggerNotification
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid ToId { get; set; }
        public bool Handled { get; set; }
        public DateTime DateTime { get; set; }

        public NotificationType NotificationType { get; set; }
        public string SerializedProperties { get; set; }
    }
}
