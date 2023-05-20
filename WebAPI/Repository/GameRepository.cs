using Microsoft.EntityFrameworkCore;
using Quartz.Core;
using Shared_Resources.Entities;
using System.Security.Policy;
using WebAPI.Interfaces;

namespace WebAPI.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly PlayerContext _playerContext;
        public GameRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public async Task<List<Game>> GetTickableGames()
        {
            var currentDate = DateTime.UtcNow;
            var games = await _playerContext.Games.Where(x => x.NextTick < currentDate && x.Active).ToListAsync();
            return games;
        }

        public async Task<Game> GetGame(Guid id)
        {
            var game = await _playerContext.Games.FirstAsync(x => x.Id == id);
            return game;
        }
    }
}
