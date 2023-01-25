using WebAPI.Enums;

namespace WebAPI.Models
{
    public class PlayerSelections
    {
        public Guid UserId { get; set; }

        public RoleType RoleType { get; set; }
        public string Name { get; set; }
    }
}
