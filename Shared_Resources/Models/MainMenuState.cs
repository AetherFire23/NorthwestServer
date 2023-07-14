using Shared_Resources.DTOs;
using System;

namespace Shared_Resources.Models;

public class MainMenuState
{
    public UserDto UserDto { get; set; } = new UserDto();
    public DateTime? TimeStamp { get; set; }
}
