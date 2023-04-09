using Shared_Resources.Entities;
using System.Collections.Generic;

namespace Shared_Resources.DTOs
{
    public class LobbyDTO // forGameState
    {
        public List<Player> PlayersInLobby { get; set; }
        public List<Player> OtherPlayers { get; set; }


    }
}
