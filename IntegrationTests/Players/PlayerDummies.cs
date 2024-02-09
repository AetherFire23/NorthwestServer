namespace IntegrationTests.Players;

public class PlayerDummies
{
    public List<PlayerInfo> GeneratePlayerDummies()
    {
        var players = new List<PlayerInfo>();
        for (int i = 0; i < 10; i++)
        {
            var registerRequest = new RegisterRequest()
            {
                Email = i.ToString(),
                UserName = i.ToString(),
                Password = i.ToString(),
            };
        }

        return players;
    }
}
