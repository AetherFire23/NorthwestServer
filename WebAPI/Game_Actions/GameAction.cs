using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Enums;

namespace WebAPI.Game_Actions
{
    public class GameAction
    {
        [Key]
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public DateTime? Created { get; set; }

        public string CreatedBy { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public GameActionType GameActionType { get; set; }

        public string SerializedProperties { get; set; }
    }
}
