using Newtonsoft.Json;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Shared_Resources.Models
{
    public static class RoomTemplate2
    {
        private static string DefaultSerializedRooms = string.Empty;
        private static Room Kitchen { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Kitchen),
            AdjacentRoomNames = new List<string>
            {
                nameof(EntryHall),
            },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = false,
        };

        private static Room EntryHall { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(EntryHall),
            AdjacentRoomNames = new List<string> { nameof(Kitchen) },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = false,
        };

        private static Room Kitchen1 { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Kitchen1),
            AdjacentRoomNames = new List<string> { nameof(Kitchen) },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = false,
        };

        // start of landmass rooms
        private static Room Cairn { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Cairn),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };
        private static Room Beach { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Beach),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };
        private static Room Plain { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Plain),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };
        private static Room Mountain { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Mountain),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };
        private static Room Village { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Village),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };
        private static Room HostileVillage { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(HostileVillage),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };
        private static Room Ziboudga { get; set; } = new Room()
        {
            Id = Guid.Empty,
            GameId = Guid.Empty,
            Name = nameof(Ziboudga),
            AdjacentRoomNames = new List<string> { },
            RoomType = RoomType.Start,
            IsActive = true,
            IsLandmass = true,
            CardImpact = Enums.CardImpact.Positive
        };

        public static void InitializeDefaultReflectedRooms() // call at start of program
        {
            List<Room> rooms = RetrieveReflectedTemplateRooms();
            string json = JsonConvert.SerializeObject(rooms);
            RoomTemplate2.DefaultSerializedRooms = json;
        }

        public static List<Room> ReadSerializedDefaultRooms()
        {
            var rooms = JsonConvert.DeserializeObject<List<Room>>(DefaultSerializedRooms) ?? new List<Room>();
            return rooms;
        }

        private static List<Room> RetrieveReflectedTemplateRooms()
        {
            var reflectedRooms = typeof(RoomTemplate2).GetProperties(BindingFlags.Static | BindingFlags.NonPublic)
                .Where(x => x.PropertyType == typeof(Room))
                .Select(x => x.GetValue(null) as Room).ToList();

            bool hasNullRoom = reflectedRooms.Any(x => x is null);
            if (hasNullRoom) throw new Exception($"One room was null while initializing !");

            var isDuplicateRoomNameOrId = reflectedRooms.FirstOrDefault(x => reflectedRooms.Where(y => x != y).Any(y => y.Name == x.Name)) != null;
            if (isDuplicateRoomNameOrId) throw new Exception($"Two rooms with the same name were found.");

            var nonNullableReflectedRooms = reflectedRooms.Select(x => x ?? new Room()).ToList();
            return nonNullableReflectedRooms;
        }
    }
}
