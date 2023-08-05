using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Services;

public class FriendService : IFriendService
{
    public ClientCallResult InviteFriend()
    {

        return ClientCallResult.Success;
    }

    public ClientCallResult AcceptOrDecline()
    {

        return ClientCallResult.Success;
    }

    public ClientCallResult RemoveFriend()
    {

        return ClientCallResult.Success;
    }
}
