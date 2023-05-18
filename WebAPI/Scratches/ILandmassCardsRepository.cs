using Shared_Resources.Entities;

namespace WebAPI.Scratches
{
    public interface ILandmassCardsRepository
    {
        public Task<List<Card>> GetLandmassCardsAsync(Guid gameId);
    }
}
