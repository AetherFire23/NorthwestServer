using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IStationRepository
    {
        public Task<StationDTO> RetrieveStationAsync<T>(Guid playerId, string stationName) where T : new(); 
        public void SaveStation(StationDTO station);
        public Task CreateAndAddStationsToDb(Guid gameId);
        public List<Station> GetAllActiveLandmassStations(Guid gameId);
        public Task<Station> GetStationByName(Guid gameId, string name);
    }
}