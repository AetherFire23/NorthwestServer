using Shared_Resources.GameTasks;
using Shared_Resources.GameTasks.Implementations_Unity;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.GameTasks.Executions
{
    [GameTask(GameTaskCodes.Cook)]
    public class CookTaskAPI : CookTaskValidate
    {
        private const string _stationNameParam = "stationName";

        private readonly IStationRepository _stationRepo;
        public CookTaskAPI(PlayerContext playerContext, IStationRepository stationRepository)
        {
            _stationRepo = stationRepository;
        }

        public override void Execute(GameTaskContext context)
        {
            string roomName = context.Parameters[_stationNameParam];
            var cookStation = _stationRepo.RetrieveStation<CookStationProperties>(context.Player.Id, roomName);

            var properties = (CookStationProperties)cookStation.ExtraProperties;

            _stationRepo.SaveStation(cookStation);
        }
    }
}
