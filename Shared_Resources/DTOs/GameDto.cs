using Shared_Resources.Interfaces;
using System;

namespace Shared_Resources.DTOs;

public class GameDto : IEntity
{
    public Guid Id { get; set; }
    public int PlayersInGameCount { get; set; }
    public DateTime Created { get; set; }
}
