using System.ComponentModel.DataAnnotations;

namespace WebAPI.Db_Models
{
    public class PrivateInvitation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FromPlayerId { get; set; }
        public Guid RoomId { get; set; }

        public string FromPlayerName { get; set; }
        public Guid ToPlayerId { get; set; }
        public string ToPlayerName { get; set; }
        public bool IsAccepted { get; set; }
        public bool RequestFulfilled { get; set; }
    }
}
