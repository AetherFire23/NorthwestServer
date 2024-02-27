namespace WebAPI.Models;

public class JoinLobbyRequest
{
    public Guid UserId { get; set; }
    public Guid LobbyId { get; set; }
}
