using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
