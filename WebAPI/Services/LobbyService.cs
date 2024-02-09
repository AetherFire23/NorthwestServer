using Shared_Resources.Entities;
using Shared_Resources.Models;
using System.Net;
using WebAPI.Exceptions;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class LobbyService
{
    private readonly PlayerContext _playerContext;
    private readonly LobbyRepository _lobbyRepository;
    private readonly UserRepository _userRepository;
    private readonly GameRepository _gameRepository;
    private readonly GameMakerService _gameMakerService;
    public LobbyService(LobbyRepository lobbyRepository,
        PlayerContext playerContext,
        UserRepository userRepository,
        GameRepository gameRepository,
        GameMakerService gameMakerService)
    {
        _lobbyRepository = lobbyRepository;
        _playerContext = playerContext;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _gameMakerService = gameMakerService;
    }

    public async Task<Lobby> CreateAndJoinLobby(Guid userId, string nameSelection)
    {
        var newLobby = await _lobbyRepository.CreateAndAddLobby();
        await CreateNewUserLobbyAndAddToDb(userId, newLobby.Id, nameSelection);

        return newLobby;
    }

    public async Task JoinLobby(JoinLobbyRequest joinRequest)
    {
        if ((await _lobbyRepository.GetLobbyById(joinRequest.LobbyId)) is null) throw new RequestException(HttpStatusCode.BadRequest);
        if (await _lobbyRepository.IsUserLobbyAlreadyExists(joinRequest.UserId, joinRequest.LobbyId)) throw new RequestException(HttpStatusCode.BadRequest);

        await CreateNewUserLobbyAndAddToDb(joinRequest.UserId, joinRequest.LobbyId, joinRequest.PlayerName);
        await CreateGameIfLobbyIsFull(joinRequest.LobbyId);
    }

    public async Task ExitLobby(Guid userId, Guid lobbyId)
    {
        Lobby? lobby = (await _lobbyRepository.GetLobbyById(lobbyId));
        if (lobby is null) throw new RequestException(HttpStatusCode.BadRequest);
        if ((await _lobbyRepository.GetUserLobbyByJoinTargetIds(userId, lobbyId)) is null) throw new RequestException(HttpStatusCode.BadRequest);

        await _lobbyRepository.DeleteUserFromLobby(userId, lobbyId);
        await _lobbyRepository.DeleteLobbyIfEmpty(lobby);

        // Send signal to update Lobbies
        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task CreateGameIfLobbyIsFull(Guid lobbyId) // important method cos it creates the game
    {
        Lobby? lobby = await _lobbyRepository.GetLobbyById(lobbyId);

        // magic number 5 to determine that game is full
        if (lobby.UsersInLobby.Count == 5)
        {
            await _gameMakerService.CreateGameFromLobby(lobbyId); // important call!
            await CleanupLobbyAndUsersAfterGameStart(lobbyId);
        }
    }

    private async Task CreateNewUserLobbyAndAddToDb(Guid userId, Guid lobbyId, string nameSelection)
    {
        var trackedUser = await _userRepository.GetUserById(userId);
        var trackedLobby = await _lobbyRepository.GetLobbyById(lobbyId);
        var userLobby = new UserLobby()
        {
            Id = Guid.NewGuid(),
            User = trackedUser,
            Lobby = trackedLobby,
            NameSelection = nameSelection
        };
        await _lobbyRepository.AddUserLobby(userLobby);
        _ = await _playerContext.SaveChangesAsync();
    }

    private async Task CleanupLobbyAndUsersAfterGameStart(Guid lobbyId)
    {
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        await _lobbyRepository.DeleteManyUsersFromLobby(lobby.UsersInLobby, lobbyId);
        _lobbyRepository.RemoveLobby(lobby);
        _ = await _playerContext.SaveChangesAsync();
    }
}
