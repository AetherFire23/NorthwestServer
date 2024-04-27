using Microsoft.EntityFrameworkCore;
using Northwest.Domain.Dtos;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Repositories;

public class GameRepository
{
    private readonly PlayerContext _playerContext;
    private readonly PlayerRepository _playerRepository;

    public GameRepository(PlayerContext playerContext, PlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
    }

    public async Task<List<Game>> GetTickableGames()
    {
        DateTime currentDate = DateTime.UtcNow;
        List<Game> games = await _playerContext.Games.Where(x => x.NextTick < currentDate && x.IsActive).ToListAsync();
        return games;
    }

    public async Task<Game> GetGameById(Guid id)
    {
        Game game = await _playerContext.Games.FirstAsync(x => x.Id == id);
        return game;
    }

    public async Task<GameDto> MapGameDto(Guid gameId)
    {
        //var gameEntity = await GetGameById(gameId);
        int playerCount = (await _playerRepository.GetPlayersInGameAsync(gameId)).Count;
        GameDto dto = new GameDto()
        {
            Id = gameId,
            PlayersInGameCount = playerCount,
            Created = DateTime.UtcNow,
        };
        return dto;
    }

    public void DeleteGame(Game game)
    {
        _ = _playerContext.Games.Remove(game);
    }
}
