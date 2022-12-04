using Microsoft.EntityFrameworkCore;
using Quartz;
using WebAPI.Interfaces;

namespace WebAPI.Jobs
{
    public class CycleJob : IJob
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICycleManagerService _cycleManager;
        public CycleJob(IGameRepository gameRepository, ICycleManagerService cycleManager)
        {
            _cycleManager = cycleManager;
            _gameRepository = gameRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tickableGames = _gameRepository.GetTickableGames();
            if (tickableGames.Count == 0)
            {
                Console.WriteLine("No games to tick.");
                return;
            }

            foreach (var game in tickableGames)
            {
                _cycleManager.TickGame(game.Id);
            }
        }
    }
}
