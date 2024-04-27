using Northwest.Domain.Dtos;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Models;

public class NewGameInfo
{
    public List<UserDto> Users { get; set; }
    public List<UserGamePreparation> UserGamePreparation { get; set; }
    public Game Game { get; set; } // created later }
    public Guid GameId => Game.Id;
}
