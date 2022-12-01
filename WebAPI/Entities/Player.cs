using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using WebAPI.Enums;

public class Player
{
    [Key]
    public Guid Id { get; set; }

    public Guid GameId { get; set; }

    public Guid CurrentChatRoomId { get; set; }

    public Guid CurrentGameRoomId { get; set; }

    public RoleType Profession { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public string Name { get; set; }

    public int HealthPoints { get; set; }

    public int ActionPoints { get; set; }

}

public enum RoomType
{
    Start,
    Second
}

namespace WebAPI.Db_Models
{
    [DataContract]
    public class PlayerDto
    {
        [DataMember]
        public float X { get; set; }

        [DataMember]
        public float Y { get; set; }

        [DataMember]
        public float Z { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}