using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Constants;

public static class TaskTargetParameterization // could be something like TargetParameters
{
    public const string StationNamePrefix = "stationName";
    public const string RoomNamePrefix = "roomName";
    public const string ItemIdPrefix = "itemId";
    public const string PlayerIdPrefix = "playerId";
    public const int ss = 0;
    public static List<Guid> GetPlayerGuids(Dictionary<string, string> parameters)
    {
        var playerIds = parameters.Where(x => x.Key.Contains("playerId"))
            .Select(x => new Guid(x.Value)).ToList();

        return playerIds;
    }

    public static List<Guid> GetItemGuids(Dictionary<string, string> parameters)
    {
        var itemIds = parameters.Where(x => x.Key.Contains("item"))
            .Select(x => new Guid(x.Value)).ToList();

        return itemIds;
    }

    public static List<string> GetRoomNames(Dictionary<string, string> parameters)
    {
        var room = parameters.Where(x => x.Key.Contains("room"))
            .Select(x => x.Value).ToList();

        return room;
    }
    public static List<string> GetStationName(Dictionary<string, string> parameters)
    {
        var room = parameters.Where(x => x.Key.Contains("station"))
            .Select(x => x.Value).ToList();

        return room;
    }
}
