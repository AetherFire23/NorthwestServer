using Shared_Resources.Entities;

namespace WebAPI.Interfaces;

public interface ILandmassCardsRepository
{
    public Task<List<Card>> GetLandmassCardsAsync(Guid gameId);
}
