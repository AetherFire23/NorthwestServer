using Shared_Resources.Entities;
using System.Collections.Generic;

namespace Shared_Resources.DTOs
{
    public class LobbyDTO // forGameState
    {
        public List<User> UsersInLobby { get; set; } = new List<User>();
    }
}
