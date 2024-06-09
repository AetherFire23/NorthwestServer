using Northwest.Domain.GameTasks.Initialization;
using Northwest.Domain.Repositories;
namespace Northwest.Domain.GameTasks;

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
        var gameTaskType = GameTaskTypeSelector.GetGameTaskType(taskCode);

        var gameTask = _serviceProvider.GetService(gameTaskType) as GameTaskBase;

        var context = await GetGameTaskExecutionContext(playerId, targets);

        var result = await gameTask.ValidateExecution(context);

        if (result.IsValid)
        {
            await gameTask.Execute(context);
        }

        return result;
    }

    public async Task<GameTaskExecutionContext> GetGameTaskExecutionContext(Guid playerId, List<List<GameTaskTargetInfo>> parameters)
    {
        var gameState = await _gameStateRepository.GetPlayerGameStateAsync(playerId, null) ?? throw new Exception();

        var context = new GameTaskExecutionContext
        {
            // ill check in integration test if this breaks
            GameState = gameState, // not tested
            Parameters = parameters,
        };

        return context;
    }
}