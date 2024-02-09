using Newtonsoft.Json;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared_Resources.Models;

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
        RoomName = nameof(RoomsTemplate.QuarterDeck),
        SerializedProperties = string.Empty,
    };

    public static Station Cannon { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Cannon),
        RoomName = nameof(RoomsTemplate.QuarterDeck),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Mizzenmast { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Mizzenmast),
        RoomName = nameof(RoomsTemplate.QuarterDeck),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Bed { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Bed),
        RoomName = nameof(RoomsTemplate.CaptainsQuarters),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Locker { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Locker),
        RoomName = nameof(RoomsTemplate.CaptainsQuarters),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };



    public static Station Watch { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Watch),
        RoomName = nameof(RoomsTemplate.CrowsNest),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Mortar { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Mortar),
        RoomName = nameof(RoomsTemplate.MainDeck),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Mainmast { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Mainmast),
        RoomName = nameof(RoomsTemplate.MainDeck),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Boat { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Boat),
        RoomName = nameof(RoomsTemplate.MainDeck),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Rail { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Rail),
        RoomName = nameof(RoomsTemplate.MainDeck),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };


    public static Station CannonForecastle { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(CannonForecastle),
        RoomName = nameof(RoomsTemplate.Forecastle),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Foremast { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Foremast),
        RoomName = nameof(RoomsTemplate.Forecastle),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Anchor { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Anchor),
        RoomName = nameof(RoomsTemplate.Forecastle),
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
        RoomName = nameof(RoomsTemplate.Galley),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Cooler { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Cooler),
        RoomName = nameof(RoomsTemplate.Galley),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Table { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Table),
        RoomName = nameof(RoomsTemplate.Mess),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Bunks { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Bunks),
        RoomName = nameof(RoomsTemplate.CrewsQuarters),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Beds { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Beds),
        RoomName = nameof(RoomsTemplate.OfficersQuarters),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Cell { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Cell),
        RoomName = nameof(RoomsTemplate.Brig),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Cabinet { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Cabinet),
        RoomName = nameof(RoomsTemplate.SickBay),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Surgery { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Surgery),
        RoomName = nameof(RoomsTemplate.SickBay),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Laboratory { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Laboratory),
        RoomName = nameof(RoomsTemplate.SickBay),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Gunpowder { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Gunpowder),
        RoomName = nameof(RoomsTemplate.Magazine),
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
        RoomName = nameof(RoomsTemplate.Hold),
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
        RoomName = nameof(RoomsTemplate.CoalStore),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };


    public static Station Provisions { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Provisions),
        RoomName = nameof(RoomsTemplate.FoodStock),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Workbench { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Workbench),
        RoomName = nameof(RoomsTemplate.Workshop),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Forge { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Forge),
        RoomName = nameof(RoomsTemplate.Workshop),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station CarpentryArea { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(CarpentryArea),
        RoomName = nameof(RoomsTemplate.Workshop),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Boiler { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Boiler),
        RoomName = nameof(RoomsTemplate.BoilerRoom),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station Engine { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(Engine),
        RoomName = nameof(RoomsTemplate.EngineRoom),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station FirearmsLocker { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(FirearmsLocker),
        RoomName = nameof(RoomsTemplate.Armory),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static Station BladesLocker { get; set; } = new Station()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        IsActive = true,
        IsLandmass = false,
        Name = nameof(BladesLocker),
        RoomName = nameof(RoomsTemplate.Armory),
        SerializedProperties = string.Empty, // Will cause mega error if properties are not created.
    };

    public static void dwdw()
    {
        List<Station> listRoosm = new List<Station>();
        listRoosm.Add(StationsTemplate.Anchor);
        listRoosm.Add(StationsTemplate.BladesLocker);
        listRoosm.Add(StationsTemplate.Bed);
        listRoosm.Add(StationsTemplate.Cabinet);
        listRoosm.Add(StationsTemplate.Bunks);
        listRoosm.Add(StationsTemplate.Gunpowder);
    }

    private static List<Station> RetrieveReflectionStations()
    {
        List<Station> reflectedStations = typeof(StationsTemplate).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
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
        List<Station> stations = JsonConvert.DeserializeObject<List<Station>>(DefaultSerializedStations) ?? new List<Station>();
        return stations;
    }
}
