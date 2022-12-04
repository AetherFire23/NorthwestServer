using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IFriendService
    {
        public ClientCallResult InviteFriend();
        public ClientCallResult AcceptOrDecline();
        public ClientCallResult RemoveFriend();
    }
}
