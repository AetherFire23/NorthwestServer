using Shared_Resources.Enums;
using System;

namespace Shared_Resources.Models
{
    public class UserGamePreparation
    {
        public Guid UserId { get; set; }
        public RoleType RoleType { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
