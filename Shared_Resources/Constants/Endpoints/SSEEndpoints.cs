using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Constants.Endpoints
{
    [ControllerPathMapper(ServerSideEvents)]
    public static class SSEEndpoints
    {
        public const string ServerSideEvents = nameof(ServerSideEvents);
        public const string EventStream = nameof(EventStream);
        
    }
}
