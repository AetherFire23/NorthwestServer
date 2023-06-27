using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Models
{
    public class MainMenuState
    {
        public List<UserDto> Friends { get; set; }
        public List<MenuNotification> Notifications { get; set; }
        public DateTime? TimeStamp { get; set; }

    }
}
