using WebAPI.GameState_Management;
using WebAPI.GameTasks.Stations;
using WebAPI.Repository;
using WebAPI.Room_template;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Cook)]
    public class CookTask : GameTaskBase
    {
        private const string _stationNameParam = "stationName";

        private readonly IStationRepository _StationRepo;
        public CookTask(PlayerContext playerContext, IStationRepository stationRepository) : base(playerContext, stationRepository)
        {
            _StationRepo = stationRepository;
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

            //SomeSharedLogic();

            var cookStation = _StationRepo.RetrieveStation<CookStationProperties>(context.Player.Id, roomName);


            _StationRepo.SaveStation(cookStation);
        }
    }
}