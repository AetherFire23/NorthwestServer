
using System;

namespace Shared_Resources.Entities
{
    public class Game
    {
        public Guid Id { get; set; } 
        public DateTime NextTick { get; set; }
        public bool Active { get; set; }
    }
}
