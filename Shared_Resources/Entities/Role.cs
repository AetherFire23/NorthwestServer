using Shared_Resources.Enums;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public RoleName RoleName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
