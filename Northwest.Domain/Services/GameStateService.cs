namespace Northwest.Domain.Services;

public class GameStateService
{
    private readonly PlayerRepository _playerRepository;
    public GameStateService(PlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
}
