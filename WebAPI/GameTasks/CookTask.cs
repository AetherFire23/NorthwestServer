using WebAPI.GameState_Management;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Cook)]
    public class CookTask : GameTaskBase
    {
        private const string _stationNameParam = "stationName"; 

        private readonly IStationRepository _stationRepo;
        public CookTask(PlayerContext playerContext, IStationRepository stationRepository) : base(playerContext, stationRepository)
        {
            _stationRepo = stationRepository;
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            //  bool containsRoomName = context.Parameters.ContainsKey(RoomNameParam);

            //if (context.Player.Z > 10)
            //{
            //    return new GameTaskValidationResult($"The current Z is {context.Player.Z} and 10 is needed.");
            //}

            return new GameTaskValidationResult();
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