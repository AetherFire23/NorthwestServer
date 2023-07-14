using System;

namespace Shared_Resources.Interfaces;

public interface IPlayerEntity : IEntity
{
    Guid GameId { get; }
}
