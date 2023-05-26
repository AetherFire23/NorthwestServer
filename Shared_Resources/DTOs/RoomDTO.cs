using Shared_Resources.Constants;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Shared_Resources.DTOs
{
    public class RoomDTO : ITaskParameter, IFormattable
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Player> Players { get; set; } = new List<Player>();
        public RoomType RoomType { get; set; }
        public List<Station> Stations { get; set; } = new List<Station>();
        public bool IsLandmass { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public KeyValuePair<string, string> GetKeyValuePairParameter(int index)
        {
            var name = $"{TaskTargetParameterization.RoomNamePrefix}{index}";
            var kvp = new KeyValuePair<string, string>(name, this.Name);
            return kvp;
        }
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
