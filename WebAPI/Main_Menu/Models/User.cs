using System.ComponentModel.DataAnnotations;

namespace WebAPI.Main_Menu.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
