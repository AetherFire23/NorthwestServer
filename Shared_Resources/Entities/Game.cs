using System;
using System.Collections.Generic;

namespace Shared_Resources.Entities;

public class Game
{
    public Guid Id { get; set; }
    public DateTime NextTick { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<Player> PlayersInGame { get; set; } = new List<Player>();


    public static int TimeBetweenTicksInSeconds = 8;
    /// <summary> Current Time + interval </summary>
    public static DateTime CalculateNextTick()
    {
        var dateTime = DateTime.UtcNow.AddSeconds(TimeBetweenTicksInSeconds);
        return dateTime;
    }


    public static Game FactorizeInitialGame()
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
