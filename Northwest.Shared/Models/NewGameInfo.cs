using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Models;

public class NewGameInfo
{
    public List<UserDto> Users { get; set; }
    public List<UserGamePreparation> UserGamePreparation { get; set; }
    public Game Game { get; set; } // created later }
    public Guid GameId => Game.Id;
}
