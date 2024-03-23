﻿using System.ComponentModel.DataAnnotations;
using WebAPI.Entities;
using WebAPI.Enums;

namespace WebAPI.DTOs;

public class PlayerDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public Guid CurrentGameRoomId { get; set; }
    public RoleType Profession { get; set; }

    [Required]
    public List<Item> Items { get; set; } = new List<Item>();
    public List<SkillEnum> Skills { get; set; } = new List<Enums.SkillEnum>();
    public string Name { get; set; } = string.Empty;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public int HealthPoints { get; set; }
    public int ActionPoints { get; set; }
}
