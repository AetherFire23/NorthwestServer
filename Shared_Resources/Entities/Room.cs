using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Entities
{
    public class Room : IEntity
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public RoomType RoomType { get; set; }
        public bool IsLandmass { get; set; }
        public bool IsActive { get; set; }


        // used to initialize adjacentRoom after extracting info from template
        //[NotMapped]
        public List<string> AdjacentRoomNames { get; set; } = new List<string>();

        //public List<AdjacentRoom> CreateRoomConnections()
        //{
        //    if (!this.AdjacentRoomNames.Any())
        //    {
        //        throw new ArgumentNullException($"{nameof(AdjacentRoomNames)} must be initialized in order to create room connections");
        //    }
        //    var newConnections = AdjacentRoomNames.Select(r =>
        //    {
        //        var newAdjacent = new AdjacentRoom()
        //        {
        //            Id = Guid.NewGuid(),
        //            AdjacentId = 
        //        }
        //    }).ToList();
        //}
    }
}
