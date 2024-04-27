using Northwest.Persistence.Entities;

namespace Northwest.Domain.Dtos;

public class LobbyDto : IEntity // forGameState
{
    public Guid Id { get; set; }
    public List<User> UsersInLobby { get; set; } = new List<User>();
}
