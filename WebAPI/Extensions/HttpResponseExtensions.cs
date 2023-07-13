using Shared_Resources.Models.SSE;

namespace WebAPI.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteAndFlushSSEData(this HttpResponse httpResponse, SSEData data)
        {
            // introdure le lock ? y se passe quoi si 2-3 methods writent en meme temps ? 
            string parseableLine = data.ConvertToReadableLine();
            await httpResponse.WriteAsync(parseableLine);
            await httpResponse.Body.FlushAsync();
        }
    }
}
