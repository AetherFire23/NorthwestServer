using Shared_Resources.Entities;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using Shared_Resources.Constants;
using System.Globalization;

namespace Shared_Resources.Entities
{
    public class Player : IEntity, ITaskParameter, IFormattable
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public Guid CurrentChatRoomId { get; set; }

        public Guid CurrentGameRoomId { get; set; }

        public RoleType Profession { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public string Name { get; set; } = string.Empty;

        public int HealthPoints { get; set; }

        public int ActionPoints { get; set; }

        public Player Clone()
        {
            return new Player()
            {
                Id = this.Id,
                ActionPoints = this.ActionPoints,
                X = this.X,
                Y = this.Y,
                Z = this.Z,
                CurrentChatRoomId = this.CurrentChatRoomId,
                CurrentGameRoomId = this.CurrentGameRoomId,
                GameId = this.GameId,
                HealthPoints = this.HealthPoints,
                Name = this.Name,
                Profession = this.Profession
            };
        }

        public KeyValuePair<string, string> GetKeyValuePairParameter(int index)
        {
            var prefix = $"{TaskTargetParameterization.PlayerIdPrefix}{index}";
            var kvp = new KeyValuePair<string, string>(prefix, this.Id.ToString());
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
