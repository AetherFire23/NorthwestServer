using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;

namespace Shared_Resources.Entities;

public class TriggerNotification
{
    //[Key]
    public Guid Id { get; set; }
    public Guid GameActionId { get; set; }
    public Guid PlayerId { get; set; }
    public bool IsReceived { get; set; }
    public DateTime Created { get; set; }

    //[Column(TypeName = "nvarchar(30)")]
    public NotificationType NotificationType { get; set; }

    public string? SerializedProperties { get; set; }

    public TriggerNotificationDTO ToDTO()
    {

        TriggerNotificationDTO notif = new TriggerNotificationDTO()
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
                    PrivateInvitationProperties? asClass = JsonConvert.DeserializeObject<PrivateInvitationProperties>(this.SerializedProperties);
                    notif.ExtraProperties = asClass;
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
