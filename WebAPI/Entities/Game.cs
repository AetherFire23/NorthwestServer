using Newtonsoft.Json;

namespace WebAPI.Entities;

public class Game
{
    public Guid Id { get; set; }
    public DateTime NextTick { get; set; }
    public bool IsActive { get; set; }

    [JsonProperty(Required = Required.AllowNull)]
    public virtual ICollection<Player> PlayersInGame { get; set; } = new List<Player>();

    public static int TimeBetweenTicksInSeconds = 8;
    /// <summary> Current Time + interval </summary>
    public static DateTime CalculateNextTick()
    {
        DateTime dateTime = DateTime.UtcNow.AddSeconds(TimeBetweenTicksInSeconds);
        return dateTime;
    }


    public static Game CreateInitialGame()
    {
        Game game = new Game()
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            NextTick = CalculateNextTick(),
        };

        return game;
    }
}
