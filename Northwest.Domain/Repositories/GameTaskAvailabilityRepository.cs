using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.GameTasks;
using Northwest.Domain.GameTasks.Initialization;
using Northwest.Domain.Models;
using Northwest.Persistence;
using SharedUtils.Extensions;

namespace Northwest.Domain.Repositories;

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
            var gameTask = _serviceProvider.GetRequiredService(taskType) as GameTaskBase ?? throw new Exception();

            (var isVisible, var gameTaskResult) = await CreateGameTaskResult(gameTask, gameState);

            if (!isVisible) continue;

            gameTaskAvailabilities.Add(gameTaskResult);
        }

        // as select statement

        var availabless = GameTaskTypeSelector.RegisteredGameTasks.Select(t =>
        {
            var gameTask = _serviceProvider.GetRequiredService(t) as GameTaskBase ?? throw new Exception();
            var gameTask2 = ServiceProviderExtensions.GetRequiredService<GameTaskBase>(_serviceProvider, t);
            var result = CreateGameTaskResult(gameTask, gameState).Result; // qestion I guess

            return result;
        })
        .Where(r => r.IsVisible)
        .Select(r => r.GameTaskResult);

        return gameTaskAvailabilities;
    }

    public async Task<(bool IsVisible, GameTaskAvailabilityResult GameTaskResult)> CreateGameTaskResult(GameTaskBase gameTask, GameState gameState)
    {
        if (!await gameTask.IsVisible(gameState)) return (false, new GameTaskAvailabilityResult());

        var requirements = await gameTask.GetTaskRequirements(gameState);

        // The reason for weird syntax is that even when the task is not executable, you need to be able to know what the gametask code is 
        if (!requirements.All(r => r.FulfillsRequirement)) return (true, new GameTaskAvailabilityResult(gameTask.Name, requirements) { GameTaskCode = gameTask.GameTaskCode });

        var prompts = await gameTask.GetGameTaskPrompts(gameState);

        var gameTaskResult = new GameTaskAvailabilityResult()
        {
            GameTaskName = gameTask.Name,
            Requirements = requirements,
            TaskPromptInfos = prompts,
            GameTaskCode = gameTask.GameTaskCode
        };

        return (true, gameTaskResult);
    }
}
