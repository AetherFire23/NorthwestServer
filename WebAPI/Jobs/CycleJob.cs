using Quartz;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Jobs;

/// <summary>
/// Handles the ticking of the games by seeking for all ongoing games
/// Then ticks them.
/// </summary>
public class CycleJob : IJob
{
    private readonly GameRepository _gameRepository;
    private readonly CycleManagerService _cycleManager;
    public CycleJob(GameRepository gameRepository, CycleManagerService cycleManager)
    {
        _cycleManager = cycleManager;
        _gameRepository = gameRepository;
    }

    public async Task Execute(IJobExecutionContext context) // en ce moment tough de pouvoir faire teser 
    {
        await Task.Delay(1);
        List<Shared_Resources.Entities.Game> tickableGames = await _gameRepository.GetTickableGames();
        if (!tickableGames.Any())
        {
            Console.WriteLine("No games to tick.");
            return;
        }

        foreach (Shared_Resources.Entities.Game game in tickableGames)
        {
            await _cycleManager.TickGame(game.Id);
        }
    }
}
