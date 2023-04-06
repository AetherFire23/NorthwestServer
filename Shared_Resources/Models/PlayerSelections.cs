﻿using Shared_Resources.Enums;

namespace Shared_Resources.Models
{
    public class PlayerSelections
    {
        public Guid UserId { get; set; }

        public RoleType RoleType { get; set; }
        public string Name { get; set; }
    }
}