using Shared_Resources.GameTasks;
using System.Net;
using WebAPI.Exceptions;
using WebAPI.GameTasks;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class GameTaskService
{
    private readonly GameStateRepository _gameStateRepository;
    private readonly IServiceProvider _serviceProvider;

    public GameTaskService(GameStateRepository gameStateRepository, IServiceProvider serviceProvider)
    {
        _gameStateRepository = gameStateRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteGameTask(Guid playerId, GameTaskCodes taskCode, TaskParameters parameters)
    {
        Shared_Resources.Models.GameState? gameState = await _gameStateRepository.GetPlayerGameStateAsync(playerId, null);
        if (gameState is null) throw new RequestException(HttpStatusCode.BadRequest);


        GameTaskContext context = new GameTaskContext
        {
            // ill check in integration test if this breaks
            GameState = gameState, // not tested
            Parameters = parameters,
        };

        Type gameTaskType = GameTaskTypeSelector.GetGameTaskType(taskCode);
        GameTaskBase? gameTask = _serviceProvider.GetService(gameTaskType) as GameTaskBase;

        GameTaskValidationResult result = gameTask.Validate(context);
        if (!result.IsValid)

            await gameTask.Execute(context);
    }
}
