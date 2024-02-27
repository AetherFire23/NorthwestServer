using Newtonsoft.Json;
using WebAPI.DTOs;
using WebAPI.Enums;
using WebAPI.Models;

namespace WebAPI.Entities;

public class TriggerNotification
{
    public Guid Id { get; set; }
    public Guid GameActionId { get; set; }
    public Guid PlayerId { get; set; }
    public bool IsReceived { get; set; }
    public DateTime Created { get; set; }
    public NotificationType NotificationType { get; set; }
    public string? SerializedProperties { get; set; }
    public TriggerNotificationDto ToDTO()
    {

        TriggerNotificationDto notif = new TriggerNotificationDto()
        {
            Id = Id,
            Created = Created,
            GameActionId = GameActionId,
            IsReceived = IsReceived,
            NotificationType = NotificationType,
            PlayerId = PlayerId,
        };
        switch (NotificationType)
        {
            case NotificationType.PrivInv:
                {
                    PrivateInvitationProperties? asClass = JsonConvert.DeserializeObject<PrivateInvitationProperties>(SerializedProperties);
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
