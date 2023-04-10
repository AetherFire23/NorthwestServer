using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Constants
{
    public static class TaskProperties
    {
        public static KeyValuePair<string, string> StationParameter
            (string stationName) => new KeyValuePair<string, string>("stationName", stationName);
    }
}
