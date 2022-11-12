using WebAPI.Main_Menu.Models;

namespace WebAPI.Repository
{
    public interface IMainMenuRepository
    {
        public MainMenuState GetMainMenuState(Guid UserId);
    }
}
