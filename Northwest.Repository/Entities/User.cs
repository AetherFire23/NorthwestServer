using System.ComponentModel.DataAnnotations.Schema;

namespace Northwest.Persistence.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public virtual ICollection<Player> Players { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    public virtual ICollection<UserLobby> UserLobbies { get; set; } = [];

    [NotMapped]
    public List<Lobby> Lobbies => UserLobbies.Any() ? UserLobbies.Select(x => x.Lobby).ToList() : new List<Lobby>();

    [NotMapped]
    public List<Game> Games => Players.Any()
        ? Players.Select(x => x.Game).ToList()
        : new List<Game>();

}