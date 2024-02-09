using Shared_Resources.Models;
namespace WebAPI.Repositories;

public class MainMenuRepository
{
    private readonly PlayerContext _playerContext;
    private readonly UserRepository _userRepository;

    public MainMenuRepository(PlayerContext playerContext, UserRepository userRepository)
    {
        _playerContext = playerContext;
        _userRepository = userRepository;
    }

    public async Task<MainMenuState> GetMainMenuState(Guid userId)
    {
        var userDto = await _userRepository.MapUserDtoById(userId);
        MainMenuState mainMenuState = new MainMenuState
        {
            UserDto = userDto,
            TimeStamp = DateTime.UtcNow,
        };

        return mainMenuState;
    }
    public bool AreFriends(Guid user1, Guid user2) // more like service bu wahtever
    {
        Shared_Resources.Entities.FriendPair? f = _playerContext.FriendPairs.FirstOrDefault(x => x.Friend1 == user1 && x.Friend2 == user2
        || x.Friend2 == user1 && x.Friend1 == user2);
        return f is not null;
    }
}