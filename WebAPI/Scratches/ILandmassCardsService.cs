using Shared_Resources.Entities;
using WebAPI.TestFolder;

namespace WebAPI.Scratches
{
    public interface ILandmassCardsService
    {
        Task<List<string>> DrawNextLandmassRoomNames(Guid gameId);
        Task<List<string>> DrawNextLandmassRoomNames2(Guid gameId, LandmassLayout layout);
        Task InitializeLandmassCards(Guid gameId);
    }
}
