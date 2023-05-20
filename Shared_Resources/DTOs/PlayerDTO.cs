using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
namespace Shared_Resources.DTOs
{
    public class PlayerDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid CurrentChatRoomId { get; set; }
        public Guid CurrentGameRoomId { get; set; }
        public RoleType Profession { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Enums.SkillEnum> Skills { get; set; } = new List<Enums.SkillEnum>();
        public string Name { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int HealthPoints { get; set; }
        public int ActionPoints { get; set; }
    }
}
