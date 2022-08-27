using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PrivateChatRoom
    {
        [Key]
        public Guid Id { get; set; }
    }
}