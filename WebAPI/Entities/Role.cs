using WebAPI.Enums;

namespace WebAPI.Entities;

public class Role
{
    public Guid Id { get; set; }

    public RoleName RoleName { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
