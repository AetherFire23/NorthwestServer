using WebAPI.Interfaces;

namespace WebAPI.Services;

public class MyStupidService
{
    private readonly IGameMakerService _gameMakerService;
    private readonly IPlayerRepository _playerRepository;

    public MyStupidService(IGameMakerService gameMakerService, IPlayerRepository playerRepository)
    {
        _gameMakerService = gameMakerService;
        _playerRepository = playerRepository;
    }
}
