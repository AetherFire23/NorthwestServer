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
            var taskParamters = new Dictionary<string, string>()
            {
                {"test,", "test2" }
            }; 

            await _state.LocalUserInfo.Client.ExecuteTaskAsync(_state.LocalUserInfo.PlayerUID, GameTaskCodes.TestTask, taskParamters);
        }
    }
}
