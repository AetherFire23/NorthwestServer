using Quartz;
using WebAPI.Interfaces;

namespace WebAPI.Jobs
{
    /// <summary>
    /// Handles the ticking of the games by seeking for all ongoing games
    /// Then ticks them.
    /// </summary>
    public class CycleJob : IJob
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICycleManagerService _cycleManager;
        public CycleJob(IGameRepository gameRepository, ICycleManagerService cycleManager)
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

            foreach (var game in tickableGames)
            {
                await _cycleManager.TickGame(game.Id);
            }
        }
    }
}
