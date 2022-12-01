using Quartz.Core;
using WebAPI.Entities;

namespace WebAPI.Interfaces
{
    public interface IGameRepository
    {
        public List<Game> GetTickableGames();
        public Game GetGame(Guid id);
    }
}
