namespace IntegrationTests.Players;

public class PlayerDummies
{
    public List<UserInfo> GeneratePlayerDummies()
    {
        List<UserInfo> players = new List<UserInfo>();
        for (int i = 0; i < 10; i++)
        {
            RegisterRequest registerRequest = new RegisterRequest()
            {
                Email = i.ToString(),
                UserName = i.ToString(),
                Password = i.ToString(),
            };
        }

        return players;
    }
}
