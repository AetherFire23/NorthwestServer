using System.ComponentModel.DataAnnotations;

namespace WebAPI.Main_Menu.Models
{
    public class FriendPair
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Friend1 { get; set; }
        public Guid Friend2 { get; set; }
    }
}
