using System;
namespace Shared_Resources.Entities
{
    public class PrivateInvitation
    {
        //[Key]
        public Guid Id { get; set; }
        public Guid FromPlayerId { get; set; }
        public Guid RoomId { get; set; }

        public string FromPlayerName { get; set; } = string.Empty;
        public Guid ToPlayerId { get; set; }
        public string ToPlayerName { get; set; } = string.Empty;
        public bool IsAccepted { get; set; }
        public bool RequestFulfilled { get; set; }
    }
}
