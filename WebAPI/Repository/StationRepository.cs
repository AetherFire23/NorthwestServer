using Newtonsoft.Json;
using WebAPI.GameTasks.Stations;
using WebAPI.Models;

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

        public StationDTO RetrieveStation(Guid playerId, string stationName)
        {
            var station = GetStation(playerId, stationName);
            var stationDTO = CreateDTO(station);
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

        private Station GetStation(Guid playerId, string stationName) // call au contexte
        {
            var player = _playerRepository.GetPlayer(playerId);
            var station = _playerContext.Stations.First(x => x.GameId == player.GameId && x.Name == stationName);
            return station;
        }

        private StationDTO CreateDTO(Station station) // From Db
        {
            return new StationDTO()
            {
                Id = station.Id,
                GameId = station.GameId,
                GameTaskCode = station.GameTaskCode,
                Name = station.Name,
                ExtraProperties = JsonConvert.DeserializeObject<object>(station.SerializedProperties),
            };
        }
    }
}