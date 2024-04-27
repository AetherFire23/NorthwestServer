using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public static class ConfiguratorControllerSerialization
{

    public static void ConfigureControllerSerialization(this IMvcBuilder builder)
    {
        builder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;

            // to convert enum to string and vice-versa to that the codegen can happen correctly 
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
    }
}
