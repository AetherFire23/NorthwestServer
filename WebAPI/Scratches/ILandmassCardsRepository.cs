using Shared_Resources.Entities;

namespace WebAPI.Scratches
{
    public interface ILandmassCardsRepository
    {
        public Task<List<Card>> GetLandmassCards(Guid gameId);
    }
}
