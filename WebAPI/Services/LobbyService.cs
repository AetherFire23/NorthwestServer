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
    private readonly GameMakerService _gameMakerService;
    public LobbyService(LobbyRepository lobbyRepository,
        PlayerContext playerContext,
        UserRepository userRepository,
        GameMakerService gameMakerService)
    {
        _lobbyRepository = lobbyRepository;
        _playerContext = playerContext;
        _userRepository = userRepository;
        _gameMakerService = gameMakerService;
    }

    public async Task<Lobby> CreateAndJoinLobby(Guid userId)
    {
        var newLobby = await _lobbyRepository.CreateAndAddLobby();
        await CreateNewUserLobbyAndAddToDb(userId, newLobby.Id);
        var lobs = _playerContext.Lobbies.ToList();
        return newLobby;
    }

    public async Task JoinLobby(JoinLobbyRequest joinRequest)
    {
        if ((await _lobbyRepository.GetLobbyById(joinRequest.LobbyId)) is null) throw new RequestException(HttpStatusCode.BadRequest);
        if (await _lobbyRepository.IsUserLobbyAlreadyExists(joinRequest.UserId, joinRequest.LobbyId)) throw new RequestException(HttpStatusCode.BadRequest);

        await CreateNewUserLobbyAndAddToDb(joinRequest.UserId, joinRequest.LobbyId);
    }

    public async Task ExitLobby(Guid userId, Guid lobbyId)
    {
        var lobby = (await _lobbyRepository.GetLobbyById(lobbyId));
        if (lobby is null) throw new RequestException(HttpStatusCode.BadRequest);
        if ((await _lobbyRepository.FindUserLobby(userId, lobbyId)) is null) throw new RequestException(HttpStatusCode.BadRequest, "user not in lobby");

        await _lobbyRepository.DeleteUserFromLobby(userId, lobbyId);
        await _lobbyRepository.DeleteLobbyIfEmpty(lobby);

        // Send signal to update Lobbies
        _ = await _playerContext.SaveChangesAsync();
    }

    // The host should decide when it is full
    public async Task StartGame(Guid lobbyId)
    {
        await _gameMakerService.CreateGameFromLobby(lobbyId); // important call!
        await CleanupLobbyAndUsersAfterGameStart(lobbyId);
    }

    public async Task CreateGameIfLobbyIsFull(Guid lobbyId) // important method cos it creates the game
    {
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);

        // magic number 5 to determine that game is full
        if (lobby.UsersInLobby.Count == 5)
        {
            await _gameMakerService.CreateGameFromLobby(lobbyId); // important call!
            await CleanupLobbyAndUsersAfterGameStart(lobbyId);
        }
    }

    private async Task CreateNewUserLobbyAndAddToDb(Guid userId, Guid lobbyId)
    {
        await _playerContext.SaveChangesAsync();
        var trackedUser = await _userRepository.GetUserById(userId);
        var trackedLobby = await _lobbyRepository.GetLobbyById(lobbyId);
        var userLobby = new UserLobby()
        {
            User = trackedUser,
            Lobby = trackedLobby,
        };

     

        _playerContext.UserLobbies.Add(userLobby);
        await _playerContext.SaveChangesAsync();
    }

    private async Task CleanupLobbyAndUsersAfterGameStart(Guid lobbyId)
    {
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        await _lobbyRepository.DeleteManyUsersFromLobby(lobby.UsersInLobby, lobbyId);
        _lobbyRepository.RemoveLobby(lobby);
        await _playerContext.SaveChangesAsync();
    }
}
