namespace WebAPI.Services;

public class MyStupidService
{
    private readonly GameMakerService _gameMakerService;
    private readonly PlayerRepository _playerRepository;

    public MyStupidService(GameMakerService gameMakerService, PlayerRepository playerRepository)
    {
        _gameMakerService = gameMakerService;
        _playerRepository = playerRepository;
    }
}
