using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Repository;

public class GameRepository : IGameRepository
{
    private readonly PlayerContext _playerContext;
    private readonly IPlayerRepository _playerRepository;

    public GameRepository(PlayerContext playerContext,
        IPlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
    }

    public async Task<List<Game>> GetTickableGames()
    {
        var currentDate = DateTime.UtcNow;
        var games = await _playerContext.Games.Where(x => x.NextTick < currentDate && x.IsActive).ToListAsync();
        return games;
    }

    public async Task<Game> GetGameById(Guid id)
    {
        var game = await _playerContext.Games.FirstAsync(x => x.Id == id);
        return game;
    }

    public async Task<GameDto> MapGameDto(Guid gameId)
    {
        var gameEntity = await GetGameById(gameId);
        var playerCount = (await _playerRepository.GetPlayersInGameAsync(gameId)).Count;
        var dto = new GameDto()
        {
            Id = gameId,
            PlayersInGameCount = playerCount,
        };
        return dto;
    }

    public void DeleteGame(Game game)
    {
        _playerContext.Games.Remove(game);
    }

}
