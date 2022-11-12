namespace WebAPI.Models.DTOs
{
    public class LobbyDTO // forGameState
    {
        public List<Player> PlayersInLobby { get; set; }
        public List<Player> OtherPlayers { get; set; }


    }
}
