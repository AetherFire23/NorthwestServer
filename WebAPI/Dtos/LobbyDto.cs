using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.DTOs;

public class LobbyDto : IEntity // forGameState
{
    public Guid Id { get; set; }
    public List<User> UsersInLobby { get; set; } = new List<User>();
}
