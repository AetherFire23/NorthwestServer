using Newtonsoft.Json;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared_Resources.Models
{
    public static class StationsTemplate
    {
        private static string DefaultSerializedStations = string.Empty;

        private static Station CookStation { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            GameTaskCode = GameTasks.GameTaskCodes.Cook,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(CookStation),
            RoomName = nameof(CookStation),
            SerializedProperties = new CookStationProperties().ToJSON(),
        };

        private static List<Station> RetrieveReflectionStations()
        {
            var reflectedStations = typeof(StationsTemplate).GetProperties(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(x => x.PropertyType == typeof(Station))
                    .Select(x => (x.GetValue(null) as Station) ?? new Station()).ToList();

            bool hasNullStation = reflectedStations.Any(x => x is null);
            if (hasNullStation) throw new Exception($"One room was null while initializing !");

            return reflectedStations;
        }

        public static void InitializeDefaultReflectedStations() // call at start of program
        {
            List<Station> rooms = RetrieveReflectionStations();
            string json = JsonConvert.SerializeObject(rooms);
            StationsTemplate.DefaultSerializedStations = json;
        }

        public static List<Station> ReadSerializedDefaultStations()
        {
            var stations = JsonConvert.DeserializeObject<List<Station>>(DefaultSerializedStations) ?? new List<Station>();
            return stations;
        }
    }
}
