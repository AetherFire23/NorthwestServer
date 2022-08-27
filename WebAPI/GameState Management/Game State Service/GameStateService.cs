namespace WebAPI.GameState_Management.Game_State_Service
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
