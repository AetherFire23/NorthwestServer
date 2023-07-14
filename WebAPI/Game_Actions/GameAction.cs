using Shared_Resources.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Game_Actions;

public class GameAction
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid GameId { get; set; }

    public DateTime? Created { get; set; } = DateTime.UtcNow;

    public string CreatedBy { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(20)")]
    public GameActionType GameActionType { get; set; }

    public string SerializedProperties { get; set; } = string.Empty;
}
