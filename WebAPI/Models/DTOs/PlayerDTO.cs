using WebAPI.Enums;

namespace WebAPI.Models.DTOs
{
    public class PlayerDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid CurrentChatRoomId { get; set; }
        public Guid CurrentGameRoomId { get; set; }
        public RoleType Profession { get; set; }
        public List<Item> Items { get; set; }
        public List<SkillType> Skills { get; set; }
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int HealthPoints { get; set; }
        public int ActionPoints { get; set; }
    }
}
