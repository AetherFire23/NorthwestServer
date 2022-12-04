using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Services
{
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

        //[HttpPut]
        //[Route("AddOrRemoveFriend")]
        //public async Task<ActionResult<ClientCallResult>> InteractWithFriends(Guid userId, FriendOptions option, [FromBody] FriendContext context)
        //{
        //    switch (option)
        //    {
        //        case FriendOptions.Invite:
        //            {
        //                var targetUser = _playerContext.Users.First(x => x.Username == context.TargetName);
        //                var targetId = targetUser.Id;
        //                var targetName = targetUser.Username;
        //                string extraProperties = JsonConvert.SerializeObject(new FriendContext()
        //                {
        //                    CallerUserId = userId,
        //                    CallerUserName = _playerContext.Users.First(x => x.Id == userId).Username,
        //                    TargetId = targetId,

        //                });
        //                var newNotification = new MenuNotification()
        //                {
        //                    MenuNotificationType = MenuNotificationType.FriendInvite,
        //                    Id = Guid.NewGuid(),
        //                    ToId = targetId,
        //                    ExtraProperties = extraProperties,
        //                    Handled = false,
        //                    Retrieved = false,
        //                    TimeStamp = DateTime.Now,
        //                };
        //                _playerContext.MenuNotifications.Add(newNotification);

        //                break;
        //            }
        //        case FriendOptions.AcceptOrDecline:
        //            {
        //                var notifications = _playerContext.MenuNotifications.Where(x => x.ToId == userId && x.MenuNotificationType == MenuNotificationType.FriendInvite).ToList();
        //                var correctNotification = new MenuNotification();
        //                var correctContext = new FriendContext();
        //                foreach (var n in notifications)
        //                {
        //                    FriendContext friendContext = JsonConvert.DeserializeObject<FriendContext>(n.ExtraProperties);
        //                    if (context.CallerUserId == friendContext.CallerUserId)
        //                    {
        //                        correctNotification = n;
        //                        correctContext = friendContext;
        //                        break;
        //                    }
        //                }

        //                var dbNotification = _playerContext.MenuNotifications.First(x => x.Id == correctNotification.Id);

        //                //Si accepted, ajoute les amis 
        //                if (context.Accepted)
        //                {
        //                    // Add new FriendPair
        //                    _playerContext.FriendPairs.Add(new FriendPair()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Friend1 = correctContext.CallerUserId,
        //                        Friend2 = correctContext.TargetId,
        //                    });
        //                }
        //                _playerContext.MenuNotifications.Remove(dbNotification);
        //                break;
        //            }
        //        case FriendOptions.Remove:
        //            {
        //                var targetUser = _playerContext.Users.First(x => x.Username == context.TargetName);
        //                var friendPair = _playerContext.FriendPairs.First(x => (x.Friend1 == userId && x.Friend2 == targetUser.Id)
        //                || (x.Friend2 == userId && x.Friend1 == targetUser.Id));
        //                _playerContext.FriendPairs.Remove(friendPair);
        //                break;
        //            }
        //    }
        //    _playerContext.SaveChanges();
        //    return Ok();
        //}
    }
}
