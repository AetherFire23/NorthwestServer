using Quartz.Core;
using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetGame(Guid id);
        Task<List<Game>> GetTickableGames();
    }
}
