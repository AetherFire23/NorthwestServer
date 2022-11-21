using WebAPI.GameTasks;
using Newtonsoft.Json;

namespace WebAPI.GameTasks.Stations
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
