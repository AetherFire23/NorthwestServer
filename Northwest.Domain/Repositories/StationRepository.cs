using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Northwest.Domain.Dtos;
using Northwest.Domain.Models;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Repositories;

public class StationRepository
{
    private readonly PlayerContext _playerContext;
    public StationRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public async Task<StationDTO> RetrieveStationAsync<T>(Guid stationId) where T : new()
    {
        Station station = await _playerContext.Stations.FirstAsync(x => x.Id == stationId);
        StationDTO stationDTO = CreateDTO<T>(station);
        return stationDTO;
    }
    public async Task<StationDTO> RetrieveStationAsync<T>(Guid gameId, string stationName) where T : new()
    {
        Station station = await _playerContext.Stations.FirstAsync(x => x.GameId == gameId && x.Name == stationName);
        StationDTO stationDTO = CreateDTO<T>(station);
        return stationDTO;
    }
    public async Task<StationDTO2<T>> RetrieveStationAsync2<T>(Guid gameId, string stationName) where T : new()
    {
        Station station = await _playerContext.Stations.FirstAsync(x => x.GameId == gameId && x.Name == stationName);
        StationDTO2<T> stationDTO = CreateDTO2<T>(station);
        return stationDTO;
    }

    public async Task SaveStation(StationDTO station)
    {
        Station currentStation = await GetStation(station.Id);
        currentStation.SerializedProperties = JsonConvert.SerializeObject(station.ExtraProperties);

        await _playerContext.SaveChangesAsync();
    }

    private async Task<Station> GetStation(Guid stationId) // call au contexte
    {
        Station station = await _playerContext.Stations.FirstAsync(x => x.Id == stationId);
        return station;
    }

    private StationDTO CreateDTO<T>(Station station) where T : new() // From Db
    {
        return new StationDTO()
        {
            Id = station.Id,
            GameId = station.GameId,
            Name = station.Name,
            ExtraProperties = JsonConvert.DeserializeObject<T>(station.SerializedProperties) ?? new T(),
        };
    }

    private StationDTO2<T> CreateDTO2<T>(Station station) where T : new() // From Db
    {
        return new StationDTO2<T>()
        {
            Id = station.Id,
            GameId = station.GameId,
            Name = station.Name,
            ExtraProperties = JsonConvert.DeserializeObject<T>(station.SerializedProperties) ?? new T(),
        };
    }

    // should create the same template as with the rooms...
    public async Task CreateAndAddStationsToDb(Guid gameId) // is template
    {
        List<Station> allStations = StationsTemplate.ReadSerializedDefaultStations();

        if (allStations.Any(x => x is null)) throw new Exception("One station was not initialized.");

        foreach (Station station in allStations)
        {
            station.GameId = gameId;

        }

        await _playerContext.Stations.AddRangeAsync(allStations);
        _ = await _playerContext.SaveChangesAsync();
    }
}