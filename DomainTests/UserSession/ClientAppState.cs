using Northwest.Domain.Models;
namespace DomainTests.UserSession;

public class ClientAppState
{
    public GameState GameState { get; set; } = null;
    public MainMenuState MainMenuState { get; set; } = null;
    
    public Guid UserId => MainMenuState.UserDto.Id;
}
