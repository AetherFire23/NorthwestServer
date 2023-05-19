using Shared_Resources.GameTasks;
using Shared_Resources.GameTasks.Implementations_Unity;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.GameTasks.Executions
{
    [GameTask(GameTaskCodes.Cook)]
    public class CookTaskExecute : CookTaskBase
    {
        private const string _stationNameParam = "stationName";
        private const int _stationCost = 3;

        private readonly IStationRepository _stationRepo;
        public CookTaskExecute(PlayerContext playerContext, IStationRepository stationRepository)
        {
            _stationRepo = stationRepository;
        }

        public override async Task Execute(GameTaskContext context)
        {
            int i = 0;
            // ca pourrait tu renvoyer le JSON au complet de la room au pire ? 
            //string roomName = context.Parameters[_stationNameParam];
            //var cookStation = await _stationRepo.RetrieveStationAsync<CookStationProperties>(context.Player.Id, roomName);

            //var properties = (CookStationProperties)cookStation.ExtraProperties;

            //_stationRepo.SaveStation(cookStation);
        }
    }
}
