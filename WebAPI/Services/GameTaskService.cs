using System.Net;
using WebAPI.Exceptions;
using WebAPI.GameTasks;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class GameTaskService
{
    private readonly GameStateRepository _gameStateRepository;
    private readonly IServiceProvider _serviceProvider;
    public GameTaskService(
        GameStateRepository gameStateRepository,
        IServiceProvider serviceProvider)
    {
        _gameStateRepository = gameStateRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task<GameTaskValidationResult> ExecuteGameTask(Guid playerId, GameTaskCodes taskCode, TaskParameters parameters)
    {
        //var context = await _gameTaskAvailabilityRepository.GetGameTaskContext(playerId, parameters);
        var context = await this.GetGameTaskContext(playerId, parameters);

        var gameTaskType = GameTaskTypeSelector.GetGameTaskType(taskCode);

        var gameTask = _serviceProvider.GetService(gameTaskType) as GameTaskBase;

        var result = await gameTask.Validate(context);

        if (result.IsValid)
        {
            await gameTask.Execute(context);
        }

        return result;
    }

    public async Task<GameTaskExecutionContext> GetGameTaskContext(Guid playerId, TaskParameters parameters)
    {
        var gameState = await _gameStateRepository.GetPlayerGameStateAsync(playerId, null) ?? throw new RequestException(HttpStatusCode.BadRequest);

        var context = new GameTaskExecutionContext
        {
            // ill check in integration test if this breaks
            GameState = gameState, // not tested
            Parameters = parameters,
        };
        return context;
    }
}
