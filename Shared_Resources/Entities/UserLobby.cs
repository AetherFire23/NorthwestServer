using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Entities
{
    public class UserLobby
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NameSelection { get; set; } // required
        public virtual User User { get; set; } = new User();
        public virtual Lobby Lobby { get; set; } = new Lobby();
    }
}
