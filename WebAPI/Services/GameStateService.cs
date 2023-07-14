using WebAPI.Interfaces;

namespace WebAPI.Services;

public class GameStateService : IGameStateService
{
    private readonly IPlayerRepository _playerRepository;
    public GameStateService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
}
