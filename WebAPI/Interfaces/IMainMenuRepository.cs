using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IMainMenuRepository
    {
        public MainMenuState GetMainMenuState(Guid UserId);
    }
}
