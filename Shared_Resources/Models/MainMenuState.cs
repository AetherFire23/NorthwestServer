using Shared_Resources.Entities;
using System.Collections.Generic;

namespace Shared_Resources.Models
{
    public class MainMenuState
    {
        public List<User> Friends { get; set; }
        public List<MenuNotification> Notifications { get; set; }
    }
}
