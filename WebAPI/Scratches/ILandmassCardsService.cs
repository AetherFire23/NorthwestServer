using Shared_Resources.Entities;

namespace WebAPI.Scratches
{
    public interface ILandmassCardsService
    {
        Task<List<string>> DrawNextLandmassRoomNames(Guid gameId);
        Task InitializeLandmassCards(Guid gameId);
    }
}
