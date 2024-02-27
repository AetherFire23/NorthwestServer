namespace WebAPI.Interfaces;

public interface ITaskParameter
{
    public (string ParamType, string Id) TaskParam { get; }
}
