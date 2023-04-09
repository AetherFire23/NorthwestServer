using System;

namespace Shared_Resources.Models
{
    public class PrivateInvitationProperties
    {
        public Guid FromPlayerId { get; set; }
        public Guid RoomId { get; set; }
        public string FromPlayerName { get; set; }
        public string ToPlayerName { get; set; }
        public bool IsAccepted { get; set; }

    }
}
