using Shared_Resources.Entities;
using System;
using System.Collections.Generic;

namespace Shared_Resources.DTOs
{
    public class LobbyDto // forGameState
    {
        public Guid Id { get; set; }
        public List<User> QueuingUsers { get; set; } = new List<User>();
    }
}
