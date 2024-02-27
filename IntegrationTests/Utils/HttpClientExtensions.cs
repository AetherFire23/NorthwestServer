namespace IntegrationTests.Utils;

public static class HttpClientExtensions
{
    public static SwagClient ToNSwagClient(this HttpClient httpClient)
    {
        SwagClient client = new SwagClient("/", httpClient);
        return client;
    }
}
