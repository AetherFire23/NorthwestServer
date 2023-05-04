using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Shared_Resources.Constants
{
    public static class TaskTargetParameterization // could be something like TargetParameters
    {
        // add prefixes through for loop plz

        // using name :
        public const string StationNamePrefix = "stationName";
        public const string RoomNamePrefix = "roomName";
        public const string ItemIdPrefix = "itemId";
        public const string PlayerIdPrefix = "playerId";




        //private static keyvaluepair<string, string> createkeyvaluepair(object parameter, int index) // getkeyvaluepair au pire ? 
        //{
        //    // specify for each type what the value conversion should be.
        //    switch (parameter)
        //    {
        //        case player player:
        //            return new keyvaluepair<string, string>($"{_playerprefix}{index}", player.id.tostring());
        //        case item item:
        //            return new keyvaluepair<string, string>($"{_itemprefix}{index}", item.id.tostring());
        //        case room room:
        //            return new keyvaluepair<string, string>($"{_roomprefix}{index}", room.name);
        //        case station station:
        //            return new keyvaluepair<string, string>($"{_stationprefix}{index}", station.name);
        //        default:
        //            throw new argumentexception("unsupported parameter type.");
        //    }
        //}


        //public static void AddAndParamerizeTargetsIntoDictionary(Dictionary<string, string> parameters, List<object> targets)
        //{
        //    var targetParameters = targets.Select((x, i) => CreateKeyValuePair(x, i)).ToList();
        //    foreach (var parameter in targetParameters)
        //    {
        //        parameters.Add(parameter.Key, parameter.Value);
        //    }
        //}

        // Open for change : peux absolument faire un JSON dans le string
        // Pourrait faire GetPlayersByGuids List<Guid> dans le repository
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
}
