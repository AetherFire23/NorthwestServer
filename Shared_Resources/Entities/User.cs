﻿
using Shared_Resources.Interfaces;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Entities
{
    public class User : IEntity, ISSESubscriber
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
