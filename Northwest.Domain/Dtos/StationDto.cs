using Northwest.Domain.GameTasks;

namespace Northwest.Domain.Dtos;

public class StationDTO
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public GameTaskCodes GameTaskCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public object ExtraProperties { get; set; } = string.Empty;
}

public class StationDTO2<T> where T : new()
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public GameTaskCodes GameTaskCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public T ExtraProperties { get; set; }
}