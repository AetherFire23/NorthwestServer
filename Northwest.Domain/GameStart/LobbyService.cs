using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Domain.Services;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
namespace Northwest.Domain.GameStart;

public class LobbyService(LobbyRepository _lobbyRepository,
        PlayerContext _playerContext,
        UserRepository _userRepository,
        GameMakerService _gameMakerService,
        JoinLobbyService joinLobbyService)
{

    public async Task<Lobby> CreateAndJoinLobby(Guid userId)
    {
        var newLobby = await joinLobbyService.CreateAndJoinLobby(userId); 
        return newLobby;
    }

    public async Task JoinLobby(JoinLobbyRequest joinRequest)
    {
        await joinLobbyService.JoinLobby(joinRequest);
    }

    public async Task ExitLobby(Guid userId, Guid lobbyId)
    {
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId)
            ?? throw new Exception();
        if (await _lobbyRepository.FindUserLobby(userId, lobbyId) is null) throw new Exception();

        await _lobbyRepository.DeleteUserFromLobby(userId, lobbyId);
        await _lobbyRepository.DeleteLobbyIfEmpty(lobby);

        
        await _playerContext.SaveChangesAsync();
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
    private async Task CleanupLobbyAndUsersAfterGameStart(Guid lobbyId)
    {
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        await _lobbyRepository.DeleteManyUsersFromLobby(lobby.UsersInLobby, lobbyId);
        _lobbyRepository.RemoveLobby(lobby);
        await _playerContext.SaveChangesAsync();
    }
}
