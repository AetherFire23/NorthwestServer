using System.Net;
using WebAPI.Exceptions;
using WebAPI.GameTasks;
using WebAPI.Models;
namespace WebAPI.Repositories;

public class GameTaskAvailabilityRepository
{
    private readonly PlayerContext _playerContext;
    private readonly IServiceProvider _serviceProvider;

    public GameTaskAvailabilityRepository(
        PlayerContext playerContext,
        IServiceProvider serviceProvider)
    {
        _playerContext = playerContext;
        _serviceProvider = serviceProvider;
    }

    // for each game task there is a gametaskVAlidationResult
    public async Task<List<GameTaskAvailabilityResult>> GetAvailableGameTasks(GameState gameState)
    {
        var gameTaskAvailabilities = new List<GameTaskAvailabilityResult>();
        foreach (var taskType in GameTaskTypeSelector.RegisteredGameTasks)
        {
            var gameTask = _serviceProvider.GetRequiredService(taskType) as GameTaskBase;
            var gameTaskResult = await gameTask.DetermineAvailability(gameState);
            gameTaskAvailabilities.Add(gameTaskResult);
        }

        return gameTaskAvailabilities;
    }
}
