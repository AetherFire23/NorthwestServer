using Microsoft.Extensions.DependencyInjection;

namespace Northwest.Domain.GameTasks.Initialization;

public class GameTaskFactory
{
    private readonly IServiceProvider _serviceProvider;

    private readonly GameTaskSelector2 _selector;

    public GameTaskFactory(IServiceProvider serviceProvider, GameTaskSelector2 selector)
    {
        _serviceProvider = serviceProvider;
        _selector = selector;
    }

    public GameTaskBase GetGameTaskFromCode(GameTaskCodes taskCode)
    {
        var type = _selector.GetGameTaskType(taskCode);
        var gameTask = _serviceProvider.GetRequiredService(type) as GameTaskBase;

        if (gameTask is null) throw new Exception("not registered");

        return gameTask;
    }
}