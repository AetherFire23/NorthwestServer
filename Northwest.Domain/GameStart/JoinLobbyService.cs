using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
namespace Northwest.Domain.GameStart;

public class JoinLobbyService(LobbyRepository _lobbyRepository, PlayerContext _playerContext, UserRepository _userRepository)
{
    internal async Task<Lobby> CreateAndJoinLobby(Guid userId)
    {
        var newLobby = await _lobbyRepository.CreateAndAddLobby();
        await CreateNewUserLobbyAndAddToDb(userId, newLobby.Id);
        return newLobby;
    }

    internal async Task JoinLobby(JoinLobbyRequest joinRequest)
    {
        await ValidateJoinLobby(joinRequest);
        await CreateNewUserLobbyAndAddToDb(joinRequest.UserId, joinRequest.LobbyId);
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

    private async Task ValidateJoinLobby(JoinLobbyRequest joinRequest)
    {
        if (await _lobbyRepository.GetLobbyById(joinRequest.LobbyId) is null) throw new Exception("Lobby does not exist");
        if (await _lobbyRepository.IsUserLobbyExists(joinRequest.UserId, joinRequest.LobbyId)) throw new Exception("User already in room");
        if (await _lobbyRepository.IsLobbyFull(joinRequest.LobbyId)) throw new Exception("Lobby is full so you cannot join it");
    }
}
