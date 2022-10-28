using WebAPI.GameState_Management;
using WebAPI.Repository;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Repair)]
    public class RepairTask : GameTaskBase
    {
        public RepairTask(PlayerContext playerContext, IStationRepository stationRepository) : base(playerContext, stationRepository)
        {

        }
        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }

        public override void Execute(GameTaskContext context)
        {
            SomeSharedLogic();
        }
    }
}