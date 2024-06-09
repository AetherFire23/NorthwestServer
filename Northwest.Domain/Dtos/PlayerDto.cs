using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;
using System.ComponentModel.DataAnnotations;

namespace Northwest.Domain.Dtos;

public class PlayerDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public Guid CurrentGameRoomId { get; set; }
    public RoleType Profession { get; set; }

    [Required]
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Skills> Skills { get; set; } = new List<Skills>();
    public string Name { get; set; } = string.Empty;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public int HealthPoints { get; set; }
    public int ActionPoints { get; set; }
}
