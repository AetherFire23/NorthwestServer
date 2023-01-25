using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IStationRepository
    {
        public StationDTO RetrieveStation<T>(Guid playerId, string stationName);
        public void SaveStation(StationDTO station);
        public StationTemplate CreateAndAddStationsToDb();
    }
}