using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RoleName> RoleNames { get; set; } = new List<RoleName>();

        // may have to rename to just Role[]
        public List<string> RoleNamesAsString => RoleNames.Select(x=> x.ToString()).ToList();
    }
}
