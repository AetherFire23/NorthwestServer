using Newtonsoft.Json;
using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Interfaces;
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

        public StationDTO RetrieveStation<T>(Guid playerId, string stationName)
        {
            var player = _playerRepository.GetPlayer(playerId);

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

        private StationDTO CreateDTO<T>(Station station) // From Db
        {
            return new StationDTO()
            {
                Id = station.Id,
                GameId = station.GameId,
                GameTaskCode = station.GameTaskCode,
                Name = station.Name,
                ExtraProperties = JsonConvert.DeserializeObject<T>(station.SerializedProperties),
            };
        }

        // pt que jai juste besoin du nom de la room a la place 
        //private StationTemplate InitializeStations(StationTemplate stationTemplate, LevelTemplate levelTemplate)
        //{
        //    // prendrait le nom de la room dans la template, faire un join 
        //}

        private StationTemplate BuildStationsTemplates()
        {
            var cookstation = new Station()
            {
                Name = "CookStation1",
                Id = Guid.NewGuid(),
                GameTaskCode = GameTasks.GameTaskCode.Cook,
                SerializedProperties = String.Empty,
            };

            var CannonStation = new Station()
            {
                Name = "CannonStation",
                Id = Guid.NewGuid(),
                GameTaskCode = GameTasks.GameTaskCode.Kill,
                SerializedProperties = String.Empty,
            };

            var stationsTemplate = new StationTemplate()
            {
                CookStation1 = cookstation,
                CannonStation = CannonStation
            };
            return stationsTemplate;
        }
    }
}