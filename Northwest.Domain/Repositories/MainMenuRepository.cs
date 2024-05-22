using Northwest.Domain.Models;
using Northwest.Persistence;

namespace Northwest.Domain.Repositories;

public class MainMenuRepository
{
    private readonly PlayerContext _playerContext;
    private readonly UserRepository _userRepository;
    private readonly GameStateRepository _gameStateRepository;

    public MainMenuRepository(PlayerContext playerContext, UserRepository userRepository, GameStateRepository gameStateRepository)
    {
        _playerContext = playerContext;
        _userRepository = userRepository;
        _gameStateRepository = gameStateRepository;
    }

    public async Task<MainMenuState> GetMainMenuState(Guid userId)
    {
        var userDto = await _userRepository.MapUserDtoById(userId);
        var mainMenuState = new MainMenuState
        {
            UserDto = userDto,
            TimeStamp = DateTime.UtcNow,
        };
        await _playerContext.SaveChangesAsync();
        return mainMenuState;
    }
    public bool AreFriends(Guid user1, Guid user2) // more like service bu wahtever
    {
        var f = _playerContext.FriendPairs.FirstOrDefault(x => x.Friend1 == user1 && x.Friend2 == user2
        || x.Friend2 == user1 && x.Friend1 == user2);
        return f is not null;
    }
}