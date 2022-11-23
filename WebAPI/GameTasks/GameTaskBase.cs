using Newtonsoft.Json;
using WebAPI.Interfaces;

namespace WebAPI.GameTasks
{
    public abstract class GameTaskBase : IGameTask 
    {
        private PlayerContext _playerContext;
        private IStationRepository _StationRepository;
        public GameTaskBase(PlayerContext playerContext, IStationRepository stationRepository)
        {
            _StationRepository = stationRepository;
            _playerContext = playerContext;
        }

        public abstract GameTaskValidationResult Validate(GameTaskContext context);
        public abstract void Execute(GameTaskContext context);

        
        protected void SomeSharedLogic()
        {
            
        }

        //protected TStation GetStation(Guid stationId)
        //{
        //    var station = _playerContext.Station.First(x=> x.Id == stationId);
            
        //    return JsonConvert.DeserializeObject<TStation>(station.SerializedStation);
        //}
    }
}