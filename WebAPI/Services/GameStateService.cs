namespace WebAPI.Services
{
    public class GameStateService
    {
        private readonly IPlayerRepository _playerRepository;
        public GameStateService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
    }
}
