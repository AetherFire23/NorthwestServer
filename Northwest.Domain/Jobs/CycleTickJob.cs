using Northwest.Domain.Repositories;
using Northwest.Domain.Services;
using Northwest.Persistence.Entities;
using Quartz;

namespace Northwest.Domain.Jobs;

/// <summary>
/// Handles the ticking of the games by seeking for all ongoing games
/// Then ticks them.
/// </summary>
public class CycleTickJob : IJob
{
    private readonly GameRepository _gameRepository;
    private readonly CycleManagerService _cycleManager;
    public CycleTickJob(GameRepository gameRepository, CycleManagerService cycleManager)
    {
        _cycleManager = cycleManager;
        _gameRepository = gameRepository;
    }

    public async Task Execute(IJobExecutionContext context) // en ce moment tough de pouvoir faire teser 
    {
        await Task.Delay(1);
        var tickableGames = await _gameRepository.GetTickableGames();

        if (!tickableGames.Any())
        {
            Console.WriteLine("No games to tick.");
            return;
        }

        foreach (Game game in tickableGames)
        {
            await _cycleManager.TickGame(game.Id);
        }
    }
}
