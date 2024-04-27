using Newtonsoft.Json;
using Northwest.Persistence.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Northwest.Persistence.Entities;

public class Player : EntityBase
{

    [JsonProperty($"id")]
    public Guid Id { get; set; }

    public Guid CurrentGameRoomId { get; set; }

    public RoleType Profession { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public string Name { get; set; } = string.Empty;

    public int HealthPoints { get; set; }

    public int ActionPoints { get; set; }

    public Guid UserId { get; set; }
    [AllowNull]
    public virtual User? User { get; set; }

    public Guid GameId { get; set; }
    public virtual Game? Game { get; set; }


    //public Player Clone()
    //{
    //    return new Player()
    //    {
    //        Id = this.Id,
    //        ActionPoints = this.ActionPoints,
    //        X = this.X,
    //        Y = this.Y,
    //        Z = this.Z,
    //        CurrentGameRoomId = this.CurrentGameRoomId,
    //        GameId = this.GameId,
    //        HealthPoints = this.HealthPoints,
    //        Name = this.Name,
    //        Profession = this.Profession
    //    };
    //}


    public override string ToString()
    {
        return ToString(null, CultureInfo.CurrentCulture);
    }


    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{Name}";
    }
}


public enum RoomType
{
    Start,
    Second
}

//namespace WebAPI.Db_Models
//{
//    [DataContract]
//    public class PlayerDto
//    {
//        [DataMember]
//        public float X { get; set; }

//        [DataMember]
//        public float Y { get; set; }

//        [DataMember]
//        public float Z { get; set; }

//        [DataMember]
//        public string Name { get; set; }
//    }
//}
