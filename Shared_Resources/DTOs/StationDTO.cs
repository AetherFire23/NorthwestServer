using Newtonsoft.Json;
using Shared_Resources.GameTasks;

namespace Shared_Resources.DTOs
{
    public class StationDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public GameTaskCode GameTaskCode { get; set; }
        public string Name { get; set; }
        public object ExtraProperties { get; set; }
    }
}
