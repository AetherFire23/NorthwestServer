using WebAPI.Interfaces;

namespace WebAPI.DTOs;

public class GameDto : IEntity
{
    public Guid Id { get; set; }
    public int PlayersInGameCount { get; set; }
    public DateTime Created { get; set; }
}
