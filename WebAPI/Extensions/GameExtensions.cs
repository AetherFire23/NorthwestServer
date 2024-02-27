using WebAPI.Entities;

namespace WebAPI.Extensions;

public static class GameExtensions
{
    public static Game GetInitialGame(this Game self)
    {
        Game game = new Game()
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            NextTick = Game.CalculateNextTick(),
        };
        return game;
    }
}
