using WebAPI.DTOs;

namespace WebAPI.Interfaces
{
    public interface IStationRepository
    {
        public StationDTO RetrieveStation<T>(Guid playerId, string stationName);
        public void SaveStation(StationDTO station);
    }
}