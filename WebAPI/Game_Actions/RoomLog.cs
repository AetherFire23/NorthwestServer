﻿using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Game_Actions
{
    public class RoomLog
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TriggeringPlayerId { get; set; } // for private logs
        public RoomLogPrivacyLevel PrivacyLevel { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string EventText { get; set; }
    }
}
