using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Models
{
    public class MainMenuState
    {
        public UserDto User { get; set; } = new UserDto();
        public List<GameDto> ActiveGames { get; set; } = new List<GameDto>();
        public List<UserDto> Friends { get; set; } = new List<UserDto>();
        public List<MenuNotification> Notifications { get; set; } = new List<MenuNotification>();
        public DateTime? TimeStamp { get; set; }
    }
}
