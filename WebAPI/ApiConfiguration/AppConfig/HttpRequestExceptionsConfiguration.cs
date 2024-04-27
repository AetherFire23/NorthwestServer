using Microsoft.AspNetCore.Diagnostics;
using Northwest.WebApi.Exceptions;

namespace Northwest.WebApi.ApiConfiguration.AppConfig;

public static class HttpRequestExceptionsConfiguration
{
    public static void ConfigureHttpRequestExceptions(this WebApplication application)
    {
        application.UseExceptionHandler(exceptionHandler =>
        {
            exceptionHandler.Run(async context =>
            {
                await SetHttpResponseDataToExceptionData(context);
            });
        });
    }

    private static async Task SetHttpResponseDataToExceptionData(HttpContext context)
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerFeature?.Error is not RequestException exBase) return;


        context.Response.StatusCode = (int)exBase.StatusCode;

        var responseData = new ExceptionResponseData()
        {
            Data = exBase.ExceptionData,
            Message = exBase.HttpMessage,
        };

        await context.Response.WriteAsJsonAsync(responseData);
    }
}
