namespace WebAPI.Entities;

public class UserLobby : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid LobbyId { get; set; }
    public Lobby Lobby { get; set; }
}

