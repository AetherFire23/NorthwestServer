using Microsoft.AspNetCore.Components.Forms;
using WebAPI.Main_Menu.Models;

namespace WebAPI.Repository
{
    public class MainMenuRepository : IMainMenuRepository
    {
        private readonly PlayerContext _playerContext;

        public MainMenuRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public MainMenuState GetMainMenuState(Guid UserId)
        {
            var friends = GetFriends(UserId);
            var notifications = GetMenuNotifications(UserId);
            var menuState = new MainMenuState()
            {
                Friends = friends,
                Notifications = notifications
            };
            return menuState;
        }

        public List<User> GetFriends(Guid userId)
        {
            var friendPairs = _playerContext.FriendPairs.Where(x => x.Friend1 == userId || x.Friend2 == userId);
            var friendIds = friendPairs.Select(x => x.Friend1 == userId ? x.Friend2 : x.Friend1);

            var friends = _playerContext.Users.Join(friendIds,
                x => x.Id,
                y => y,
                (x, y) => new User()
                {
                    Id = x.Id,
                    Username = x.Username
                }).ToList();

            return friends;
        }

        public List<MenuNotification> GetMenuNotifications(Guid userId)
        {
            var notifications = _playerContext.MenuNotifications.Where(x => x.ToId == userId
            && x.Retrieved == false);
            foreach (var notification in notifications)
            {
                notification.Retrieved = true;
            }
            return notifications.ToList();
            _playerContext.SaveChanges();
        }

        public User GetUser(Guid userId)
        {
            var user = _playerContext.Users.First(x => x.Id == userId);
            return user;
        }

        public bool AreFriends(Guid user1, Guid user2) // more like service bu wahtever
        {
            var f = _playerContext.FriendPairs.FirstOrDefault(x => (x.Friend1 == user1 && x.Friend2 == user2)
            || (x.Friend2 == user1 && x.Friend1 == user2));
            return f is not null;
        }
    }
}