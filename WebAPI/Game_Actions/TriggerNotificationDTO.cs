using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Game_Actions
{
    public class TriggerNotificationDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid ToId { get; set; }
        public bool Handled { get; set; } // faut trouver uen facon que les
        // animations jouent pour tout le monde, mmh.
        // playerId pour tout le monde ? pis dans le fond les global ca ferait juste mettre plein de playerId

        public NotificationType NotificationType { get; set; }
        public object ExtraProperties { get; set; }
    }
}
