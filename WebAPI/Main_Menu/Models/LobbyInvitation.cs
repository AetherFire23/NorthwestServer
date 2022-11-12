using System.ComponentModel.DataAnnotations;

namespace WebAPI.Main_Menu.Models
{
    public class LobbyInvitation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FromPlayerId { get; set; } // localPlayer
        public Guid LobbyId { get; set; } // whateverId ?
        public string FromPlayerName { get; set; }
        public Guid ToPlayerId { get; set; }
        public string ToPlayerName { get; set; } // invitedUser
        public bool IsAccepted { get; set; }
        public bool RequestFulfilled { get; set; }
    }
}
