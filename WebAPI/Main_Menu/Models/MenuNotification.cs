using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Main_Menu.Models
{
    public class MenuNotification
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ToId { get; set; }
        public bool Retrieved { get; set; } // modified quand le state est retrieved
        public bool Handled { get; set; }
        public DateTime? TimeStamp { get; set; }
        public MenuNotificationType MenuNotificationType { get; set; }
        public string ExtraProperties { get; set; }
    }
}
