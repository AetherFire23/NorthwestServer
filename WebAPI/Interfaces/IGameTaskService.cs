using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using Shared_Resources.Scratches;

namespace WebAPI.Interfaces;

public interface IGameTaskService
{
    public Task<ClientCallResult> ExecuteGameTask(Guid playerId, GameTaskCodes taskCode, TaskParameters parameters);


}
