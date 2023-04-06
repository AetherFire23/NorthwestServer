﻿using Shared_Resources.Entities;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class MenuNotification
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ToId { get; set; }
        public bool Retrieved { get; set; } // modified quand le state est retrieved
        public bool Handled { get; set; }
        public DateTime? TimeStamp { get; set; }
        public MenuNotificationType MenuNotificationType { get; set; }
        public string ExtraProperties { get; set; }
    }
}