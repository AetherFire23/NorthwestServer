
using Newtonsoft.Json;
using Shared_Resources.Interfaces;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Entities
{
    public class User : IEntity, ISSESubscriber
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public virtual ICollection<Game> Games { get; set; }  = new List<Game>();
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserLobby> UserLobbies { get; set; } = new List<UserLobby>();
        public List<Lobby> Lobbies => UserLobbies.Select(x=> x.Lobby).ToList();
    }
}
