using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;

namespace Shared_Resources.DTOs;

public class LobbyDto : IEntity // forGameState
{
    public Guid Id { get; set; }
    public List<User> QueuingUsers { get; set; } = new List<User>();
}
