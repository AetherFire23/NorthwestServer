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

        public StationTemplate CreateAndAddStationsToDb()
        {
            var cookstation = new Station()
            {
                Name = "CookStation1",
                Id = Guid.NewGuid(),
                GameTaskCode = GameTasks.GameTaskCode.Cook,
                SerializedProperties = SerializedDefaultCookStation,
            };

            var CannonStation = new Station()
            {
                Name = "CannonStation",
                Id = Guid.NewGuid(),
                GameTaskCode = GameTasks.GameTaskCode.Kill,
                SerializedProperties = SerializedDefaultCannonProperty,
            };

            var stationsTemplate = new StationTemplate()
            {
                CookStation1 = cookstation,
                CannonStation = CannonStation
            };

            var stations = typeof(StationTemplate)
                .GetProperties();
            var stations2 = stations
                .Select(property => property.GetValue(stationsTemplate))
                .Select(obj => (Station)obj).ToList();

            if (stations2.Any(x => x is null)) throw new Exception("One station was not initialized.");

            _playerContext.Stations.AddRange(stations2);
            _playerContext.SaveChanges();
            return stationsTemplate;
        }

        public static string SerializedDefaultCannonProperty = JsonConvert.SerializeObject(new CannonProperties()
        {
            State = Enums.State.Pristine,
            Ammo = 5,
        });

        public static string SerializedDefaultCookStation = JsonConvert.SerializeObject(new CookStationProperties()
        {
            MoneyMade = 5,
            State = Enums.State.Pristine,
        });
    }
}