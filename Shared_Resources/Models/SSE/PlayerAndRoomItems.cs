using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Models.SSE
{
    public class PlayerAndRoomItems
    {
        public List<Item> PlayerItems { get; set; } = new List<Item>();
        public List<Item> RoomItems { get; set; } = new List<Item>();
    }
}
