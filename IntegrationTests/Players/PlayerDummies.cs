namespace IntegrationTests.Players;

public class PlayerDummies
{
    public List<UserInfo> GeneratePlayerDummies()
    {
        var players = new List<UserInfo>();
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
