using System;
namespace Shared_Resources.DTOs;

public class StationDTO
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public GameTaskCodes GameTaskCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public object ExtraProperties { get; set; } = string.Empty;
}
