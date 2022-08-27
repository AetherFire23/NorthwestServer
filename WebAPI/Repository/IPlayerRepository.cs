
using WebAPI;
using WebAPI.Models;

public interface IPlayerRepository
{
    public List<string> GetPlayerNames();
    public List<Player> GetPlayers();

    public Player GetPlayerByName(string name);

    public Player GetPlayerById(Guid id);
    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId);
    public List<PrivateInvitation> GetPlayerInvitations(Player player);
    public List<Player> GetPlayersInCurrentGame(Player player);
    public List<Player> GetPlayersInCurrentGame(Guid playerId);
}

