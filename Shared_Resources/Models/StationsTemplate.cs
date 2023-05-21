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

        public static Station Wheel { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Wheel),
            RoomName = nameof(RoomTemplate2.QuarterDeck),
            SerializedProperties = string.Empty,
        };

        public static Station Cannon { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Cannon),
            RoomName = nameof(RoomTemplate2.QuarterDeck),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Mizzenmast { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Mizzenmast),
            RoomName = nameof(RoomTemplate2.QuarterDeck),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Bed { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Bed),
            RoomName = nameof(RoomTemplate2.CaptainsQuarters),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Locker { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Locker),
            RoomName = nameof(RoomTemplate2.CaptainsQuarters),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };



        public static Station Watch { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Watch),
            RoomName = nameof(RoomTemplate2.CrowsNest),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Mortar { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Mortar),
            RoomName = nameof(RoomTemplate2.MainDeck),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Mainmast { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Mainmast),
            RoomName = nameof(RoomTemplate2.MainDeck),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Boat { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Boat),
            RoomName = nameof(RoomTemplate2.MainDeck),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Rail { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Rail),
            RoomName = nameof(RoomTemplate2.MainDeck),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        //Forecastle

        public static Station CannonForecastle { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(CannonForecastle),
            RoomName = nameof(RoomTemplate2.Forecastle),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Foremast { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Foremast),
            RoomName = nameof(RoomTemplate2.Forecastle),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Anchor { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Anchor),
            RoomName = nameof(RoomTemplate2.Forecastle),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        // Galley
        public static Station Stove { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Stove),
            RoomName = nameof(RoomTemplate2.Galley),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Cooler { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Cooler),
            RoomName = nameof(RoomTemplate2.Galley),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Table { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Table),
            RoomName = nameof(RoomTemplate2.Mess),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Bunks { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Bunks),
            RoomName = nameof(RoomTemplate2.CrewsQuarters),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Beds { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Beds),
            RoomName = nameof(RoomTemplate2.OfficersQuarters),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Cell { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Cell),
            RoomName = nameof(RoomTemplate2.Brig),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Cabinet { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Cabinet),
            RoomName = nameof(RoomTemplate2.SickBay),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Surgery { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Surgery),
            RoomName = nameof(RoomTemplate2.SickBay),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Laboratory { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Laboratory),
            RoomName = nameof(RoomTemplate2.SickBay),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Gunpowder { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Gunpowder),
            RoomName = nameof(RoomTemplate2.Magazine),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };


        // Lower Deck

        public static Station ScrapPile { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(ScrapPile),
            RoomName = nameof(RoomTemplate2.Hold),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        // Coal Store
        public static Station Coal { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Coal),
            RoomName = nameof(RoomTemplate2.CoalStore),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };


        public static Station Provisions { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Provisions),
            RoomName = nameof(RoomTemplate2.FoodStock),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Workbench { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Workbench),
            RoomName = nameof(RoomTemplate2.Workshop),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Forge { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Forge),
            RoomName = nameof(RoomTemplate2.Workshop),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station CarpentryArea { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(CarpentryArea),
            RoomName = nameof(RoomTemplate2.Workshop),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Boiler { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Boiler),
            RoomName = nameof(RoomTemplate2.BoilerRoom),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station Engine { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(Engine),
            RoomName = nameof(RoomTemplate2.EngineRoom),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station FirearmsLocker { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(FirearmsLocker),
            RoomName = nameof(RoomTemplate2.Armory),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        public static Station BladesLocker { get; set; } = new Station()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            IsActive = true,
            IsLandmass = false,
            Name = nameof(BladesLocker),
            RoomName = nameof(RoomTemplate2.Armory),
            SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        };

        //public static Station CookStation { get; set; } = new Station()
        //{
        //    Id = Guid.Empty,
        //    GameId = Guid.Empty,
        //    IsActive = true,
        //    IsLandmass = false,
        //    Name = nameof(CookStation),
        //    RoomName = nameof(RoomTemplate2.),
        //    SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
        //};

        private static List<Station> RetrieveReflectionStations()
        {
            var reflectedStations = typeof(StationsTemplate).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
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
