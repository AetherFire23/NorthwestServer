using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.DTOs;
using WebAPI.Enums;

namespace WebAPI.Db_Models
{
    public class TriggerNotification
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameActionId { get; set; }
        public Guid PlayerId { get; set; }
        public bool IsReceived { get; set; }
        public DateTime Created { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public NotificationType NotificationType { get; set; }

        public string? SerializedProperties { get; set; }

        public TriggerNotificationDTO ToDTO()
        {
            return new TriggerNotificationDTO
            {
                Id = Id,
                GameActionId = GameActionId,
                PlayerId = PlayerId,
                IsReceived = IsReceived,
                Created = Created,
                NotificationType = NotificationType,
                ExtraProperties = JsonConvert.DeserializeObject<object>(SerializedProperties ?? "{}")
            };
        }
    }
}
