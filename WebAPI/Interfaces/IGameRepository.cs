using Quartz.Core;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface IGameRepository
    {
        void DeleteGame(Game game);
        Task<Game> GetGameById(Guid id);
        Task<List<Game>> GetTickableGames();
        Task<GameDto> MapGameDto(Guid gameId);
    }
}
