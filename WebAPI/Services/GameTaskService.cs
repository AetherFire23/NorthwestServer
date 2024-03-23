using System.Net;
using System.Runtime.CompilerServices;
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

    public async Task<GameTaskValidationResult> ExecuteGameTask(Guid playerId, GameTaskCodes taskCode, List<List<GameTaskTargetInfo>> targets)
    {
        int i = 3;
        double d = i;

        //var context = await _gameTaskAvailabilityRepository.GetGameTaskContext(playerId, parameters);
        var context = await this.GetGameTaskExecutionContext(playerId, targets);

        var gameTaskType = GameTaskTypeSelector.GetGameTaskType(taskCode);

        var gameTask = _serviceProvider.GetService(gameTaskType) as GameTaskBase;

        var result = await gameTask.ValidateExecution(context);

        if (result.IsValid)
        {
            await gameTask.Execute(context);
        }

        return result;
    }

    public async Task<GameTaskExecutionContext> GetGameTaskExecutionContext(Guid playerId, List<List<GameTaskTargetInfo>> parameters)
    {
        Models.GameState gameState = await _gameStateRepository.GetPlayerGameStateAsync(playerId, null) ?? throw new RequestException(HttpStatusCode.BadRequest);

        GameTaskExecutionContext context = new GameTaskExecutionContext
        {
            // ill check in integration test if this breaks
            GameState = gameState, // not tested
            Parameters = parameters,
        };





        return context;
    }
}


public class Vector3
{
    public int X { get; set; }
    public int Y { get; set; }

    public static implicit operator Vector2(Vector3 vec3) => new Vector2(vec3.X, vec3.Y);
}

public class Vector2
{
    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

}
