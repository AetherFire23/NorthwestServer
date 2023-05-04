using Shared_Resources.DTOs;
using WebAPI.TestFolder;

namespace WebAPI.Interfaces
{
    public interface ILandmassRepository
    {
        public LandmassLayout GetRandomLandmassLayout();

        public LandmassRoomsDeck GetCurrentLandmassDeckSetup(Guid gameId);
        public void SaveLandmassLayout(Guid gameId, LandmassLayout layout);
        public void SavePreviousLandmass(Guid gameId);
        public void SaveDecksSetup(LandmassRoomsDeck decksSetup);

    }
}
