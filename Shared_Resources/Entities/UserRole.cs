using System;

namespace Shared_Resources.Entities;

public class UserRole
{
    public Guid Id { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}
