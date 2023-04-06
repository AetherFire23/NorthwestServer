using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Models;

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