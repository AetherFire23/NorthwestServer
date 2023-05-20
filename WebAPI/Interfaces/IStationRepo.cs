using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IStationRepository
    {
        Task CreateAndAddStationsToDb(Guid gameId);
        Task<StationDTO> RetrieveStationAsync<T>(Guid stationId) where T : new();
        Task<StationDTO> RetrieveStationAsync<T>(Guid gameId, string stationName) where T : new();
        Task SaveStation(StationDTO station);
    }
}