using WebAPI.Db_Models;

namespace WebAPI.Models
{
    public class MainMenuState
    {
        public List<User> Friends { get; set; }
        public List<MenuNotification> Notifications { get; set; }
    }
}
