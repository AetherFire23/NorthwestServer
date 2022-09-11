using WebAPI.GameState_Management;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Clean)]
    public class CleanTask : GameTaskBase
    {
        private const string TargetIdKey = "TargetId";
        private readonly IPlayerRepository _playerRepository;

        public CleanTask(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            if (!context.Parameters.ContainsKey(TargetIdKey))
            {
                return new GameTaskValidationResult($"The target id is missing from the paramters");
            }

            string targetId = context.Parameters[TargetIdKey];
            var targetPlayer = _playerRepository.GetPlayerById(Guid.Parse(targetId));

            if (targetPlayer == null)
            {
                return new GameTaskValidationResult($"The target {targetId} does not exist.");
            }

            return new GameTaskValidationResult();
        }

        public override void Execute(GameTaskContext context)
        {
            SomeSharedLogic();
        }
    }
}