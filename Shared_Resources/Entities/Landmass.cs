using System;

namespace Shared_Resources.Entities
{
    public class Landmass
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string SerializedLandmassLayout { get; set; } = string.Empty;

        
    }
}
