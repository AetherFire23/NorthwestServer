

using Shared_Resources.Models;

namespace WebAPI.Interfaces;

public interface IMainMenuRepository
{
    //public MainMenuState GetMainMenuState(Guid UserId);
    Task<MainMenuState> GetMainMenuState(Guid userId);
}
