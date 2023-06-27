using Microsoft.AspNetCore.Components.Forms;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Repository
{
    public class MainMenuRepository : IMainMenuRepository
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;
        public MainMenuRepository(PlayerContext playerContext, IPlayerRepository playerRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
        }

        // presume logged in I could guess
        public async Task<MainMenuState> GetMainMenuState(Guid playerId) // almost could pass in 
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);

            var mainMenuState = new MainMenuState
            {
                TimeStamp = DateTime.UtcNow,
            };

            return mainMenuState;
        }

        //public async Task<List<MenuNotification>> GetMenuNotifications(Guid userId)
        //{
        //    var notifications = _playerContext.MenuNotifications.Where(x => x.ToId == userId
        //    && x.Retrieved == false);
        //    foreach (var notification in notifications)
        //    {
        //        notification.Retrieved = true;
        //    }
        //    await _playerContext.SaveChangesAsync();

        //    return notifications.ToList();
        //}

        public bool AreFriends(Guid user1, Guid user2) // more like service bu wahtever
        {
            var f = _playerContext.FriendPairs.FirstOrDefault(x => (x.Friend1 == user1 && x.Friend2 == user2)
            || (x.Friend2 == user1 && x.Friend1 == user2));
            return f is not null;
        }
    }
}