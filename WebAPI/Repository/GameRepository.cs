using Quartz.Core;
using System.Security.Policy;
using WebAPI.Entities;
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
        public List<Game> GetTickableGames()
        {
            var currentDate = DateTime.UtcNow;
            var games = _playerContext.Games.Where(x => x.NextTick < currentDate && x.Active).ToList();
            return games;
        }
        public Game GetGame(Guid id)
        {
            var game = _playerContext.Games.First(x => x.Id == id);
            return game;
        }
    }
}
