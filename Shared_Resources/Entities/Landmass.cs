using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class Landmass
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string SerializedLandmassLayout { get; set; } = string.Empty;

        
    }
}
