using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PrivateInvitationNotification
    {
        public Guid FromPlayerId { get; set; }
        public Guid RoomId { get; set; }
        public string FromPlayerName { get; set; }
        public string ToPlayerName { get; set; }
        public bool IsAccepted { get; set; }
    }
}
