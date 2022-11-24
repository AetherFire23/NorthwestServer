using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class FriendContext
    {
        public Guid CallerUserId { get; set; }
        public string CallerUserName { get; set; }
        public string TargetName { get; set; }
        public Guid TargetId { get; set; }
        public bool Accepted { get; set; }
    }
}
