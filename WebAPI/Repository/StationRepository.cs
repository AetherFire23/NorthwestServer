using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.GameTasks;
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

        public async Task<Station> GetStationByName(Guid gameId, string name)
        {
            var station = await _playerContext.Stations.FirstAsync(s => s.GameId == gameId && s.Name == name);
            return station;
        }

        public List<Station> GetAllActiveLandmassStations(Guid gameId)
        {
            var stations = _playerContext.Stations.Where(s => s.IsActive && s.GameId == gameId).ToList();
            return stations;
        }

        public async Task<StationDTO> RetrieveStationAsync<T>(Guid playerId, string stationName) where T : new()
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);
            var station = _playerContext.Stations.First(x => x.GameId == player.GameId && x.Name == stationName);
            var stationDTO = CreateDTO<T>(station);
            return stationDTO;
        }

        public void SaveStation(StationDTO station)
        {
            var currentStation = GetStation(station.Id);
            currentStation.SerializedProperties = JsonConvert.SerializeObject(station.ExtraProperties);

            _playerContext.SaveChanges();
        }

        private Station GetStation(Guid stationId) // call au contexte
        {
            var station = _playerContext.Stations.First(x => x.Id == stationId);
            return station;
        }

        private StationDTO CreateDTO<T>(Station station) where T : new() // From Db
        {
            return new StationDTO()
            {
                Id = station.Id,
                GameId = station.GameId,
                GameTaskCode = station.GameTaskCode,
                Name = station.Name,
                ExtraProperties = JsonConvert.DeserializeObject<T>(station.SerializedProperties) ?? new T(),
            };
        }

        // should create the same template as with the rooms...
        public async Task CreateAndAddStationsToDb(Guid gameId) // is template
        {
            var allStations = StationsTemplate.ReadSerializedDefaultStations();

            if (allStations.Any(x => x is null)) throw new Exception("One station was not initialized.");

            _playerContext.Stations.AddRange(allStations);
            await _playerContext.SaveChangesAsync();
        }
    }
}