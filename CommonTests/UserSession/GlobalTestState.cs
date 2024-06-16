using Northwest.Domain.Repositories;

namespace DomainTests.UserSession;

public class GlobalTestState
{
    public ClientAppState LocalPlayerState { get; set; }
    public List<ClientAppState> ClientAppStates { get; set; }
    public List<ClientAppState> OtherAppStates => ClientAppStates.Where(x => LocalUserId != x.UserId).ToList();
    public Guid LocalUserId => LocalPlayerState.MainMenuState.UserDto.Id;
}