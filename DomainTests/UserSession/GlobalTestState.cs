using Northwest.Domain.Repositories;

namespace DomainTests.UserSession;

public class GlobalTestState
{
    public ClientAppState LocalPlayerState { get; set; }
    public List<ClientAppState> UserStates { get; set; }

    public Guid LocalUserId => LocalPlayerState.MainMenuState.UserDto.Id;
}