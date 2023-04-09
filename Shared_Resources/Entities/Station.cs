using Shared_Resources.GameTasks;
using System.Runtime.CompilerServices;
using Shared_Resources.GameTasks;
using System;

namespace Shared_Resources.Entities
{
    public class Station
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public GameTaskCodes GameTaskCode { get; set; }
        public string Name { get; set; }
        public string SerializedProperties { get; set; } // code is in the gamestak to know in which class to convert this
        public bool IsLandmass { get; set; }
        public bool IsActive { get; set; }
    }
}
