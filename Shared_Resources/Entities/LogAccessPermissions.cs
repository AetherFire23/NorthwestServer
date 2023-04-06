using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class LogAccessPermissions
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LogId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
