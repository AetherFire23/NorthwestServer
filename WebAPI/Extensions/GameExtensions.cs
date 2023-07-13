using Shared_Resources.Entities;
using WebAPI.Services;

namespace WebAPI.Extensions
{
    public static class GameExtensions
    {
        public static Game GetInitialGame(this Game self)
        {
            var game = new Game()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                NextTick = Game.CalculateNextTick(),
            };
            return game;
        }
    }
}
