
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IFriendService
    {
        public ClientCallResult InviteFriend();
        public ClientCallResult AcceptOrDecline();
        public ClientCallResult RemoveFriend();
    }
}
