using Northwest.Domain.Models;
using Northwest.WebApi.ApiConfiguration.BuilderConfig;
namespace Northwest.WebApi;

public class Program
{
    static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
    {
        var builder = WebApplication.CreateBuilder(args);
        ApplicationBuilderHelper.ConfigureWebApplication(builder);

        var app = builder.Build();




        app.UseHttpLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}