using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Interfaces;
using WebAPI.Repository.Users;

namespace WebAPI.Services;

public class LobbyService : ILobbyService
{
    private readonly PlayerContext _playerContext;
    private readonly ILobbyRepository _lobbyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IGameMakerService _gameMakerService;
    public LobbyService(ILobbyRepository lobbyRepository,
        PlayerContext playerContext,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        IGameMakerService gameMakerService)
    {
        _lobbyRepository = lobbyRepository;
        _playerContext = playerContext;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _gameMakerService = gameMakerService;
    }

    public async Task<ClientCallResult> CreateAndJoinLobby(Guid userId, string nameSelection)
    {
        var newLobby = await _lobbyRepository.CreateAndAddLobby();
        await CreateNewUserLobbyAndAddToDb(userId, newLobby.Id, nameSelection);

        var clientCallResult = new ClientCallResult()
        {
            IsSuccessful = true,
            Content = newLobby,
        };

        return clientCallResult;
    }

    public async Task<ClientCallResult> JoinLobby(JoinLobbyRequest joinRequest)
    {
        if ((await _lobbyRepository.GetLobbyById(joinRequest.LobbyId)) is null) return ClientCallResult.Failure;
        if (await _lobbyRepository.IsUserLobbyAlreadyExists(joinRequest.UserId, joinRequest.LobbyId)) return ClientCallResult.Failure;

        await CreateNewUserLobbyAndAddToDb(joinRequest.UserId, joinRequest.LobbyId, joinRequest.PlayerName);
        await CreateGameIfLobbyIsFull(joinRequest.LobbyId);
        // Start game if the lobby is full
        // SSE to update lobbies
        return ClientCallResult.Success;
    }

    public async Task<ClientCallResult> ExitLobby(Guid userId, Guid lobbyId)
    {
        var lobby = (await _lobbyRepository.GetLobbyById(lobbyId));
        if (lobby is null) return ClientCallResult.Failure;
        if ((await _lobbyRepository.GetUserLobbyByJoinTargetIds(userId, lobbyId)) is null) return new ClientCallResult() { IsSuccessful = false, Message = "User is not in this lobby" };

        await _lobbyRepository.DeleteUserFromLobby(userId, lobbyId);
        await _lobbyRepository.DeleteLobbyIfEmpty(lobby);

        // Send signal to update Lobbies
        _ = await _playerContext.SaveChangesAsync();
        return ClientCallResult.Success;
    }

    public async Task CreateGameIfLobbyIsFull(Guid lobbyId) // important method cos it creates the game
    {
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        if (lobby.UsersInLobby.Count == 5)
        {
            await _gameMakerService.CreateGameFromLobby(lobbyId); // important call lol!
            await CleanupLobbyAndUsersAfterGameStart(lobbyId);


        }
    }

    private async Task CreateNewUserLobbyAndAddToDb(Guid userId, Guid lobbyId, string nameSelection)
    {
        // Join tables using just the virtual syntax and leveraging ef core shadowing sshit require entities to be tracked/
        // requires superfluous db calls but who care
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
