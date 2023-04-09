using Newtonsoft.Json;
using Shared_Resources.GameTasks;
using System;
using System.Collections.Generic;
namespace Shared_Resources.DTOs
{
    public class StationDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public GameTaskCodes GameTaskCode { get; set; }
        public string Name { get; set; }
        public object ExtraProperties { get; set; }
    }
}
