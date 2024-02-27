using WebAPI.DTOs;

namespace WebAPI.GameTasks;

public class TaskParameters : List<(string ParamType, string Id)>
{
    public TaskParameters()
    {

    }

    public TaskParameters(Dictionary<string, string> taskParams)
    {
        foreach (var taskParam in taskParams)
        {
            this.Add((taskParam.Key, taskParam.Value));
        }
    }

    public TaskParameters(List<(string ParamType, string Id)> list)
    {
        AddRange(list);
    }

    public TaskParameters(List<Tuple<string, string>> list)
    {
        AddRange(list.Select(x =>
        {
            (string ParamType, string Id) kvp = (x.Item1, x.Item2);
            return kvp;
        }));
    }

    public List<(string ParamType, string Id)> GetRooms()
    {
        List<(string ParamType, string Id)> rooms = this.Where(x => x.ParamType == typeof(RoomDto).Name).ToList();
        return rooms;
    }
}
