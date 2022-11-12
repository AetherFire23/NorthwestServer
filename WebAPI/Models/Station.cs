using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using WebAPI.GameTasks;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class Station
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public GameTaskCode GameTaskCode { get; set; }
        public string Name { get; set; }
        public string SerializedProperties { get; set; } // code is in the gamestak to know in which class to convert this
    }
}
