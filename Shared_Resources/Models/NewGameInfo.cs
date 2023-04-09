
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
namespace Shared_Resources.Models
{
    public class NewGameInfo
    {
        // required for starting initialization
        public Game Game { get; set; }
        public List<User> Users { get; set; }
        public List<PlayerSelections> RoleChoices { get; set; }

        public List<Player> Players { get; set; }

        public List<Room> Rooms { get; set; }
    }
}
