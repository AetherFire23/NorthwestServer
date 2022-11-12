using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Game_Actions
{
    public class GameAction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public DateTime? TimeStamp { get; set; }
        public GameActionType GameActionType { get; set; }
        public string SerializedProperties { get; set; }
    }
}
