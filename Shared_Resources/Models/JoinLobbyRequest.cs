using System;

namespace Shared_Resources.Models;

public class JoinLobbyRequest
{
    public Guid UserId { get; set; }
    public Guid LobbyId { get; set; }
    public string PlayerName { get; set; } = string.Empty; // when the game starts, which name to apply to the player 
}
