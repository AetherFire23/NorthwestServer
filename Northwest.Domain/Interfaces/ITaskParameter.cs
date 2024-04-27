namespace Northwest.Domain.Interfaces;

public interface ITaskParameter
{
    public (string ParamType, string Id) TaskParam { get; }
}
