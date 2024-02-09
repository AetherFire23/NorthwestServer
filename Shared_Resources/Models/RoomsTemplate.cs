using Newtonsoft.Json;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared_Resources.Models;

// TODO toute rentre ca public pour pas toute casser et pouvoir linker les tasks apres
public static class RoomsTemplate // cest quoi deja les stairway haha
{
    private static string DefaultSerializedRooms = string.Empty;
    public static Room CrowsNest { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(CrowsNest),
        AdjacentRoomNames = new List<string>
        {
            nameof(MainDeck),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room MainDeck { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(MainDeck),
        AdjacentRoomNames = new List<string>
        {
            nameof(CrowsNest),
            nameof(Forecastle),
            nameof(FrontStairway),
            nameof(CrowsNest),
            nameof(QuarterDeck),
            nameof(RearStairway),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Forecastle { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Forecastle),
        AdjacentRoomNames = new List<string>
        {
            nameof(MainDeck),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room ChartsRoom { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(ChartsRoom),
        AdjacentRoomNames = new List<string>
        {
            nameof(FrontStairway),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room QuarterDeck { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(QuarterDeck),
        AdjacentRoomNames = new List<string>
        {
            nameof(MainDeck),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room CaptainsQuarters { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(CaptainsQuarters),
        AdjacentRoomNames = new List<string>
        {
            nameof(RearStairway),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room FrontStairway { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(FrontStairway),
        AdjacentRoomNames = new List<string>
        {
            nameof(ChartsRoom),
            nameof(MainDeck),
            nameof(Mess),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room RearStairway { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(RearStairway),
        AdjacentRoomNames = new List<string>
        {
            nameof (MainDeck),
            nameof (CaptainsQuarters),
            nameof (MiddleCorridor),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room SickBay { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(SickBay),
        AdjacentRoomNames = new List<string>
        {
            nameof (Brig),
            nameof (Mess),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Brig { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Brig),
        AdjacentRoomNames = new List<string>
        {
            nameof (SickBay),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Mess { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Mess),
        AdjacentRoomNames = new List<string>
        {
            nameof (SickBay),
            nameof (CrewsQuarters),
            nameof (FrontStairway),
            nameof (MiddleCorridor),
            nameof (Galley),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room MiddleCorridor { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(MiddleCorridor),
        AdjacentRoomNames = new List<string>
        {
            nameof (Mess),
            nameof (OfficersQuarters),
            nameof (LaundryRoom),
            nameof (RearStairway),
            nameof (LowerStairway),
            nameof (Magazine),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room CrewsQuarters { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(CrewsQuarters),
        AdjacentRoomNames = new List<string>
        {
            nameof (Mess),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Galley { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Galley),
        AdjacentRoomNames = new List<string>
        {
            nameof (Mess),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Magazine { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Magazine),
        AdjacentRoomNames = new List<string>
        {
            nameof(MiddleCorridor),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room OfficersQuarters { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(OfficersQuarters),
        AdjacentRoomNames = new List<string>
        {
            nameof(MiddleCorridor),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room LaundryRoom { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(LaundryRoom),
        AdjacentRoomNames = new List<string>
        {
            nameof(LaundryRoom),
            nameof(MiddleCorridor),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room LowerStairway { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(LowerStairway),
        AdjacentRoomNames = new List<string>
        {
            nameof(MiddleCorridor),
            nameof(LowerCorridor),
        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Armory { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Armory),
        AdjacentRoomNames = new List<string>
        {
            nameof(LowerCorridor),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room EngineRoom { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(EngineRoom),
        AdjacentRoomNames = new List<string>
        {
            nameof(BoilerRoom),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Workshop { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Workshop),
        AdjacentRoomNames = new List<string>
        {
            nameof(LowerCorridor),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room BoilerRoom { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(BoilerRoom),
        AdjacentRoomNames = new List<string>
        {
            nameof(LowerCorridor),
            nameof(EngineRoom),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room Hold { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Hold),
        AdjacentRoomNames = new List<string>
        {
            nameof(LowerCorridor),
            nameof(CoalStore),
            nameof(FoodStock),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room CoalStore { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(CoalStore),
        AdjacentRoomNames = new List<string>
        {
            nameof(Hold),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room FoodStock { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(FoodStock),
        AdjacentRoomNames = new List<string>
        {
            nameof(Hold),

        },
        IsActive = true,
        IsLandmass = false,
    };

    public static Room LowerCorridor { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(LowerCorridor),
        AdjacentRoomNames = new List<string>
        {
            nameof(LowerStairway),
            nameof(Armory),
            nameof(EngineRoom),
            nameof(Workshop),
            nameof(BoilerRoom),
            nameof(Hold),
        },
        IsActive = true,
        IsLandmass = false,
    };


    //private static Room MyRoom { get; set; } = new Room()
    //{
    //    Id = Guid.Empty,
    //    GameId = Guid.Empty,
    //    Name = nameof(MyRoom),
    //    AdjacentRoomNames = new List<string>
    //    {
    //    },
    //    RoomType = RoomType.Start,
    //    IsActive = true,
    //    IsLandmass = false,
    //};

    // start of landmass rooms
    public static Room Cairn { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Cairn),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };
    public static Room Beach { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Beach),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };
    public static Room Plain { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Plain),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };
    public static Room Mountain { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Mountain),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };
    public static Room Village { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Village),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };
    public static Room HostileVillage { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(HostileVillage),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };
    public static Room Ziboudga { get; set; } = new Room()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Name = nameof(Ziboudga),
        AdjacentRoomNames = new List<string> { },
        IsActive = true,
        IsLandmass = true,
        CardImpact = Enums.CardImpact.Positive
    };

    public static void InitializeDefaultReflectedRooms() // call at start of program
    {
        List<Room> rooms = DiscoverReflectedTemplateRooms();
        string json = JsonConvert.SerializeObject(rooms);
        RoomsTemplate.DefaultSerializedRooms = json;
    }

    public static List<Room> ReadSerializedDefaultRooms()
    {
        List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(DefaultSerializedRooms) ?? new List<Room>();
        return rooms;
    }

    private static List<Room> DiscoverReflectedTemplateRooms()
    {
        List<Room?> reflectedRooms = typeof(RoomsTemplate).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(x => x.PropertyType == typeof(Room))
            .Select(x => x.GetValue(null) as Room).ToList();

        bool hasNullRoom = reflectedRooms.Any(x => x is null);
        if (hasNullRoom) throw new Exception($"One room was null while initializing !");

        bool isDuplicateRoomNameOrId = reflectedRooms.FirstOrDefault(x => reflectedRooms.Where(y => x != y).Any(y => y.Name == x.Name)) != null;
        if (isDuplicateRoomNameOrId) throw new Exception($"Two rooms with the same name were found.");

        List<Room> nonNullableReflectedRooms = reflectedRooms.Select(x => x ?? new Room()).ToList();
        return nonNullableReflectedRooms;
    }
}
