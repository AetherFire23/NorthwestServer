using Shared_Resources.DTOs;
using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface ILobbyRepository
    {
        Task AddLobby(Lobby lobber);
        Task AddUserLobby(UserLobby userLobby);
        Task<Lobby> CreateAndAddLobby();
        Task DeleteLobbyIfEmpty(Lobby lobby);
        Task<Lobby> GetLobbyById(Guid lobbyId);
        Task<UserLobby?> GetUserLobbyByJoinTargetIds(Guid userId, Guid lobbyId);
        Task<bool> IsUserLobbyAlreadyExists(Guid userId, Guid lobbyId);
        Task<LobbyDto> MapLobbyDto(Guid lobbyId);
        void RemoveLobby(Lobby entity);
        Task DeleteUserFromLobby(Guid userId, Guid lobbyId);
        Task DeleteManyUsersFromLobby(List<User> users, Guid lobbyId);
    }
}