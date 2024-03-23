using IntegrationTests.Players;

namespace IntegrationTests.GameTask
{
    public class TestGameTasks
    {
        private readonly TestState _state;

        public TestGameTasks(TestState state)
        {
            _state = state;
        }

        public async Task DoTestTask()
        {
            var parameters = new List<List<GameTaskTargetInfo>>()
            {
                { new List<GameTaskTargetInfo>() {
                    new GameTaskTargetInfo()
                    {
                        Id = Guid.NewGuid(),
                        Name = "yeah",
                    }
                }
            }};

            await _state.LocalUserInfo.Client.ExecuteTaskAsync(_state.LocalUserInfo.PlayerUID, GameTaskCodes.TestTaskNoTargets, parameters);
        }
    }
}
