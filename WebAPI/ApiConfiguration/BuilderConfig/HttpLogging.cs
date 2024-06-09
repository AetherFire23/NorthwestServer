namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public static class HttpLogging
{
    public static void ConfigureHTTPLogging(this IServiceCollection serviceCollection)
    {
        //serviceCollection.AddHttpLogging(logging =>
        //{
        //    logging.LoggingFields = HttpLoggingFields.All;
        //    _ = logging.RequestHeaders.Add("sec-ch-ua");
        //    _ = logging.ResponseHeaders.Add("MyResponseHeader");
        //    logging.MediaTypeOptions.AddText("application/javascript");
        //    logging.RequestBodyLogLimit = 4096;
        //    logging.ResponseBodyLogLimit = 4096;
        //});

        serviceCollection.AddHttpLogging(options => { });
    }
}
