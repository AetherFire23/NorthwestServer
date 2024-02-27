namespace WebAPI.Interfaces;

public interface IPlayerEntity : IEntity
{
    Guid GameId { get; }
}
