using Shared_Resources.GameTasks;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IGameTaskService
    {
        public Task<ClientCallResult> ExecuteGameTask(Guid playerId, GameTaskCodes taskCode, Dictionary<string, string> parameters);


    }
}
