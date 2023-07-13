using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using Shared_Resources.Models.SSE.TransferData;
using WebAPI.Interfaces;
using WebAPI.Repository.Users;
using WebAPI.SSE.ClientManagers;
using WebAPI.SSE.SSECore;

namespace WebAPI.SSE.Senders
{
    public class MainMenuSSESender : SSESenderBase<MainMenuClientManager>, IMainMenuSSESender
    {
        private readonly ILobbyRepository _lobbyRepository;
        private readonly IUserRepository _userRepository;

        public MainMenuSSESender(IServiceProvider serviceProvider,
            ILobbyRepository lobbyRepository,
            IUserRepository userRepository) : base(serviceProvider)
        {
            _lobbyRepository = lobbyRepository;
            _userRepository = userRepository;
        }

        public async Task RefreshLobbiesAndGamesInfo(Guid userId)
        {
            var lobbies = await _userRepository.GetLobbyDtosForUser(userId);
            var activeGames = await _userRepository.GetActiveGameDtosForUser(userId);

            var sseData = new SSEData(SSEType.RefreshLobbiesAndActiveGames, new RefreshLobbiesAndGamesTransfer()
            {
                ActiveGamesForUser = activeGames,
                QueuedLobbies = lobbies,
            });

            await _client.PushDataToSubscriber(userId, sseData);
        }
    }
}
