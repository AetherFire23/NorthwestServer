using System.ComponentModel.DataAnnotations;

namespace WebAPI.Main_Menu.Models
{
    public class LobbyParticipant
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LobbyId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
