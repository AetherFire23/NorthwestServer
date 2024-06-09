﻿using Northwest.Persistence.Entities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace Northwest.Domain.Dtos;

public class RoomDto : EntityBase
{
    public (string ParamType, string Id) TaskParam => (GetType().Name, Id.ToString());

    public Guid GameId { get; set; }
    public string Name { get; set; } = string.Empty;

    [Required]
    public List<Item> Items { get; set; } = [];
    public List<Player> Players { get; set; } = [];
    public RoomType RoomType { get; set; }
    public List<Station> Stations { get; set; } = new List<Station>();
    public bool IsLandmass { get; set; }

    public float X { get; set; }
    public float Y { get; set; }

    public override string ToString()
    {
        return ToString(null, CultureInfo.CurrentCulture);
    }

    public string ToString(string? format, IFormatProvider formatProvider)
    {
        return $"{Name}";
    }
}
