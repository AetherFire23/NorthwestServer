using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared_Resources.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Game_Actions
{
    public class GameAction
    {
        [Key]
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public DateTime? Created { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(20)")]
        public GameActionType GameActionType { get; set; }

        public string SerializedProperties { get; set; } = string.Empty;
    }
}
