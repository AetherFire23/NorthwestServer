﻿namespace WebAPI.Models.DTOs
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public List<Item> Items { get; set; }
        public List<Player> Players { get; set; }
        public RoomType RoomType { get; set; }
    }
}
