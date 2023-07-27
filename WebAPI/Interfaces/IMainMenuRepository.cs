

using Shared_Resources.Models;

namespace WebAPI.Interfaces;

public interface IMainMenuRepository
{
    Task<ClientCallResult> GetMainMenuState(Guid userId);
}
