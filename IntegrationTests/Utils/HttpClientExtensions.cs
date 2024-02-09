namespace IntegrationTests.Utils;

public static class HttpClientExtensions
{
    public static SwagClient ToNSwagClient(this HttpClient httpClient)
    {
        var client = new SwagClient("/", httpClient);
        return client;
    }
}
