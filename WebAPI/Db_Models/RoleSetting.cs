using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Db_Models
{
    public class RoleSetting
    {
        [Key]
        public Guid Id { get; set; }
        public RoleType RoleType { get; set; }
        public string SerializedSettings { get; set; }
    }
}
