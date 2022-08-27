using Microsoft.EntityFrameworkCore;
using System.Text;
using WebAPI;
using WebAPI.Models;

public class PlayerRepository : IPlayerRepository
{
    PlayerContext _playerContext;
    DbSet<Player> Players => _playerContext.Players;
    public PlayerRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public List<string> GetPlayerNames()
    {
        return _playerContext.Players.Select(player => player.Name).ToList();
    }

    public List<Player> GetPlayers()
    {
        return _playerContext.Players.ToList();
    }

    public Player GetPlayerByName(string name)
    {
        Player? candidate = _playerContext.Players.FirstOrDefault(player => player.Name.Equals(name));

        if (candidate is null)
        {
            throw new NotImplementedException("dude wtf");
        }

        return candidate;
    }

    public Player GetPlayerById(Guid id)
    {
      //  return _playerContext.Players.Where(queriedPlayer => queriedPlayer.Id == id).First();
        return _playerContext.Players.FirstOrDefault(queriedPlayer => queriedPlayer.Id == id);
    }

    public List<PrivateInvitation> GetPlayerInvitations(Guid playerId)
    {
        var playerInvites = _playerContext.Invitations.Where(invite => invite.ToPlayerId == playerId).ToList();
        return playerInvites;
    }
    public List<PrivateInvitation> GetPlayerInvitations(Player player)
    {
        var playerInvites = _playerContext.Invitations.Where(invite => invite.ToPlayerId == player.Id).ToList();
        return playerInvites;
    }

    public List<Player> GetPlayersInCurrentGame(Player player)
    {
        var players = _playerContext.Players.Where(qPlayer => qPlayer.GameId == player.GameId).ToList();
        return players;
    }

    public List<Player> GetPlayersInCurrentGame(Guid playerId)
    {
        var player = _playerContext.Players.FirstOrDefault(qPlayer => qPlayer.Id == playerId);
        var players = _playerContext.Players.Where(qPlayer => qPlayer.GameId == player.GameId).ToList();
        return players;
    }

    public void ChangeRooms(Player player, RoomType roomtype)
    {

    }
}