using Shared_Resources.GameTasks;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IGameTaskService
    {
        public ClientCallResult ExecuteGameTask(Guid playerId, GameTaskCode taskCode, Dictionary<string, string> parameters);


    }
}
