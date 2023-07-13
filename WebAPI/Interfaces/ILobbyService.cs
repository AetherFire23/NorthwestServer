using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface ILobbyService
    {
        Task<ClientCallResult> CreateAndJoinLobby(Guid userId, string nameSelection);
        Task<ClientCallResult> JoinLobby(JoinLobbyRequest joinRequest);
        Task<ClientCallResult> ExitLobby(Guid userId, Guid lobbyId);
        Task CreateGameIfLobbyIsFull(Guid lobbyId);
    }
}
