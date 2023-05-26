using Shared_Resources.Enums;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Shared_Resources.Entities
{
    public class Room : IEntity, IFormattable
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsLandmass { get; set; }
        public bool IsActive { get; set; }
        public List<string> AdjacentRoomNames { get; set; } = new List<string>();

        public float X { get; set; }
        public float Y { get; set; }

        //NotMapped!
        public CardImpact CardImpact { get; set; } = CardImpact.Neutral; // should be NotMapped in database since it is for initializing the landmass rooms


        // public virtual List<Station> Stations { get; set; } = new List<Station>();

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

        public override string ToString()
        {
            return ToString(null, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{this.Name}";
        }
    }
}
