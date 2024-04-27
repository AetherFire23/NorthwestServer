using Northwest.Persistence.Enums;
using System.ComponentModel.DataAnnotations;

namespace Northwest.Persistence.Entities;

public class GameAction
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid GameId { get; set; }

    public DateTime? Created { get; set; } = DateTime.UtcNow;

    public string CreatedBy { get; set; } = string.Empty;

    public GameActionType GameActionType { get; set; }

    public string SerializedProperties { get; set; } = string.Empty;
}
