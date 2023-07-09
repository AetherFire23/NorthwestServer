using System;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using System.Collections.Generic;

namespace Shared_Resources.Models
{
    public class GameState
    {
        public PlayerDTO PlayerDTO { get; set; } = new PlayerDTO();
        public RoomDTO Room { get; set; } = new RoomDTO(); // ark va etre called a eter delted
        public List<Message> NewMessages { get; set; } = new List<Message>();
        public List<Player> Players { get; set; } = new List<Player>();
        public List<TriggerNotificationDTO> TriggerNotifications { get; set; } = new List<TriggerNotificationDTO>();
        public List<PrivateChatRoomParticipant> PrivateChatRoomParticipants { get; set; } = new List<PrivateChatRoomParticipant>();
        public List<Log> Logs { get; set; } = new List<Log>();
        public List<RoomDTO> Rooms { get; set; } = new List<RoomDTO>();
        public DateTime? TimeStamp { get; set; }
        public string SerializedLayout { get; set; } = string.Empty;
        public List<PrivateChatRoom> PrivateChatRooms { get; set; } = new List<PrivateChatRoom>();
        public List<Station> Stations { get; set; } = new List<Station>();

        public Guid GameId => this.PlayerDTO.GameId;
        public Guid PlayerUID => this.PlayerDTO.Id;
    }
}