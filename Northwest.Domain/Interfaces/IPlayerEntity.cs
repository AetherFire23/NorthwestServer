using Northwest.Persistence.Entities;

namespace Northwest.Domain.Interfaces;

public interface IPlayerEntity : IEntity
{
    Guid GameId { get; }
}
