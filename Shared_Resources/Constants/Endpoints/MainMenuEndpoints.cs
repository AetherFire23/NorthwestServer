using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Shared_Resources.Constants.Endpoints
{
    // private shouldnt be retrieved NOR used
    // should add some ToLower shit in the reflection
    // must be constant to be used in another attribute.

    [ControllerPathMapper(MainMenu)]
    public static class MainMenuEndpoints
    {
        public const string MainMenu = nameof(MainMenu);
        public const string State = nameof(State);
        public const string CreateLobby = nameof(CreateLobby);
    }
}
