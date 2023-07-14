namespace WebAPI.Interfaces;

public interface IGameMakerService
{
    Task CreateGameFromLobby(Guid lobbyId);
}
