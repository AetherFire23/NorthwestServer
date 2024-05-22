using Northwest.Domain.Repositories;

namespace DomainTests.UserSession;

public class TestEnvironmentStatesUpdater(GameStateRepository gameStateRepository, MainMenuRepository mainMenuRepository)
{
    public async Task UpdateState(ClientAppState clientAppState)
    {
        var gameState = await gameStateRepository.GetPlayerGameStateAsync(clientAppState.MainMenuState.UserDto.Players[0].Id, clientAppState.GameState.TimeStamp);

        var mainmenuState = await mainMenuRepository.GetMainMenuState(clientAppState.MainMenuState.UserDto.Id);

        clientAppState.GameState = gameState;

        clientAppState.MainMenuState = mainmenuState;
    }
}