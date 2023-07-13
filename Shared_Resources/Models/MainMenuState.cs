using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Models
{
    public class MainMenuState
    {
        public UserDto UserDto { get; set; } = new UserDto();
        public DateTime? TimeStamp { get; set; }
    }
}
