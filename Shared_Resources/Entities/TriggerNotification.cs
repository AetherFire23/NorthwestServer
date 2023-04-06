﻿using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shared_Resources.Entities
{
    public class TriggerNotification
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameActionId { get; set; }
        public Guid PlayerId { get; set; }
        public bool IsReceived { get; set; }
        public DateTime Created { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public NotificationType NotificationType { get; set; }

        public string? SerializedProperties { get; set; }

        public TriggerNotificationDTO ToDTO()
        {

            var notif = new TriggerNotificationDTO()
            {
                Id = this.Id,
                Created = this.Created,
                GameActionId = this.GameActionId,
                IsReceived = this.IsReceived,
                NotificationType = this.NotificationType,
                PlayerId = this.PlayerId,
            };
            switch (this.NotificationType)
            {
                case NotificationType.PrivInv:
                    {
                        var asClass = JsonConvert.DeserializeObject<PrivateInvitationProperties>(this.SerializedProperties);
                        notif.ExtraProperties = (object)asClass;
                        break;
                    }
                case NotificationType.CycleChanged:
                    {
                        break;
                    }
                case NotificationType.DoorOpen:
                    {
                        break;
                    }
            }

            return notif;
        }
    }
}