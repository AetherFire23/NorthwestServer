using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;

namespace WebAPI.Repositories;

public class LandmassCardsRepository
{
    private readonly PlayerContext _playerContext;
    public LandmassCardsRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public async Task<List<Card>> GetLandmassCardsAsync(Guid gameId)
    {
        List<Card> cards = await _playerContext.Cards.Where(x => x.GameId == gameId).ToListAsync();
        return cards;
    }
}
