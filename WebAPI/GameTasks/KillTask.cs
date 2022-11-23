using WebAPI.Interfaces;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Kill)]
    public class KillTask : GameTaskBase
    {
        public KillTask(PlayerContext playerContext, IStationRepository stationRepository) : base(playerContext, stationRepository)
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