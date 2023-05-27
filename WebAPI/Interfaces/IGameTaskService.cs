using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using Shared_Resources.Scratches;
using System.Security.Permissions;

namespace WebAPI.Interfaces
{
    public interface IGameTaskService
    {
        public Task<ClientCallResult> ExecuteGameTask(Guid playerId, GameTaskCodes taskCode, TaskParameters parameters);

        
    }
}
