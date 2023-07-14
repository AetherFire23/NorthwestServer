using Shared_Resources.DTOs;

namespace WebAPI.Interfaces;

public interface IStationRepository
{
    Task CreateAndAddStationsToDb(Guid gameId);
    Task<StationDTO> RetrieveStationAsync<T>(Guid stationId) where T : new();
    Task<StationDTO> RetrieveStationAsync<T>(Guid gameId, string stationName) where T : new();
    Task SaveStation(StationDTO station);
}