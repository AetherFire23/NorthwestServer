using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Entities;

public class Lobby : IEntity
{
    public Guid Id { get; set; }

    public virtual ICollection<UserLobby> UserLobbies { get; set; } = new List<UserLobby>();
    public List<User> UsersInLobby => UserLobbies.Select(x => x.User).ToList();
}
