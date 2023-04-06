using Quartz.Core;
using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface IGameRepository
    {
        public List<Game> GetTickableGames();
        public Game GetGame(Guid id);
    }
}
