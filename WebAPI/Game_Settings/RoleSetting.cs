using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.GameActions
{
    public class RoleSetting
    {
        [Key]
        public Guid Id { get; set; }
        public RoleType RoleType { get; set; }
        public string SerializedSettings { get; set; }
    }
}
