using WebAPI.GameTasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IGameTaskService
    {
        public ClientCallResult ExecuteGameTask(Guid playerId, GameTaskCode taskCode, Dictionary<string, string> parameters);


    }
}
