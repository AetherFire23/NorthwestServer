using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Repository
{
    public class StationRepository : IStationRepository
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;

        public StationRepository(PlayerContext playerContext, IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _playerContext = playerContext;
        }

        public async Task<StationDTO> RetrieveStationAsync<T>(Guid stationId) where T : new()
        {
            var station = await _playerContext.Stations.FirstAsync(x => x.Id == stationId);
            var stationDTO = CreateDTO<T>(station);
            return stationDTO;
        }
        public async Task<StationDTO> RetrieveStationAsync<T>(Guid gameId, string stationName) where T : new()
        {
            var station = await _playerContext.Stations.FirstAsync(x => x.GameId == gameId && x.Name == stationName);
            var stationDTO = CreateDTO<T>(station);
            return stationDTO;
        }

        public async Task SaveStation(StationDTO station)
        {
            var currentStation = await GetStation(station.Id);
            currentStation.SerializedProperties = JsonConvert.SerializeObject(station.ExtraProperties);

            await _playerContext.SaveChangesAsync();
        }

        private async Task<Station> GetStation(Guid stationId) // call au contexte
        {
            var station = await _playerContext.Stations.FirstAsync(x => x.Id == stationId);
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

        // should create the same template as with the rooms...
        public async Task CreateAndAddStationsToDb(Guid gameId) // is template
        {
            var allStations = StationsTemplate.ReadSerializedDefaultStations();

            if (allStations.Any(x => x is null)) throw new Exception("One station was not initialized.");

            foreach (var station in allStations)
            {
                station.GameId = gameId;

            }

            await _playerContext.Stations.AddRangeAsync(allStations);
            await _playerContext.SaveChangesAsync();
        }
    }
}