using Microsoft.AspNetCore.Mvc;
using WebAPI;

public class PlayerService : IPlayerService
{
    PlayerContext _playerContext;
    IPlayerRepository _playerRepository;
    public PlayerService(PlayerContext playerContext, IPlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
    }
    public void UpdatePlayerPosition(Player player)
    {
        Player selectedPlayer = _playerRepository.GetPlayer(player.Id);
        selectedPlayer.X = player.X;
        selectedPlayer.Y = player.Y;
        selectedPlayer.Z = player.Z;
        _playerContext.SaveChanges();
    }
}