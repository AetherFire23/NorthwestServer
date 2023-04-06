

using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IStationRepository
    {
        public StationDTO RetrieveStation<T>(Guid playerId, string stationName);
        public void SaveStation(StationDTO station);
        public StationTemplate CreateAndAddStationsToDb(Guid gameId);

        public Station GetStationByName(Guid gameId, string name);
        public List<Station> GetAllActiveLandmassStations(Guid gameId);

    }
}