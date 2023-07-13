using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IGameMakerService
    {
        Task CreateGameFromLobby(Guid lobbyId);
    }
}
