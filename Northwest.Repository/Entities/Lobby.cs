using System.ComponentModel.DataAnnotations.Schema;

namespace Northwest.Persistence.Entities;

public class Lobby : IEntity
{
    public Guid Id { get; set; }

    public virtual ICollection<UserLobby> UserLobbies { get; set; } = [];

    [NotMapped]
    public List<User> UsersInLobby => UserLobbies.Select(x => x.User).ToList();
}
