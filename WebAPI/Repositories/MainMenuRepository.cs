using Shared_Resources.Models;
using WebAPI.Interfaces;
using WebAPI.Repository.Users;

namespace WebAPI.Repository;

public class MainMenuRepository : IMainMenuRepository
{
    private readonly PlayerContext _playerContext;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ILobbyRepository _lobbyRepository;

    public MainMenuRepository(PlayerContext playerContext,
        IPlayerRepository playerRepository,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        ILobbyRepository lobbyRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _lobbyRepository = lobbyRepository;
    }

    public async Task<ClientCallResult> GetMainMenuState(Guid userId)
    {
        var userDto = await _userRepository.MapUserDtoById(userId);
        var mainMenuState = new MainMenuState
        {
            UserDto = userDto,
            TimeStamp = DateTime.UtcNow,
        };
        var clientCallResult = new ClientCallResult()
        {
            IsSuccessful = true,
            Content = mainMenuState,
        };

        return clientCallResult;
    }
    public bool AreFriends(Guid user1, Guid user2) // more like service bu wahtever
    {
        var f = _playerContext.FriendPairs.FirstOrDefault(x => (x.Friend1 == user1 && x.Friend2 == user2)
        || (x.Friend2 == user1 && x.Friend1 == user2));
        return f is not null;
    }
}