using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.DTOs
{
    public class MenuNotificationDTO
    {
        public Guid Id { get; set; }
        public Guid ToId { get; set; }
        public bool Retrieved { get; set; } // modified quand le state est retrieved
        public bool Handled { get; set; }
        public DateTime? TimeStamp { get; set; }
        public MenuNotificationType MenuNotificationType { get; set; }
        public object ExtraProperties { get; set; }
    }
}
