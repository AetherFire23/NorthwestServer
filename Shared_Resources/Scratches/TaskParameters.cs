using Shared_Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Scratches;

public class TaskParameters : List<(string ParamType, string Id)>
{
    public TaskParameters()
    {

    }

    public TaskParameters(List<(string ParamType, string Id)> list)
    {
        this.AddRange(list);
    }

    public TaskParameters(List<Tuple<string, string>> list)
    {
        this.AddRange(list.Select(x =>
        {
            (string ParamType, string Id) kvp = (x.Item1, x.Item2);
            return kvp;
        }));
    }

    public List<(string ParamType, string Id)> GetRooms()
    {
        var rooms = this.Where(x => x.ParamType == typeof(RoomDTO).Name).ToList();
        return rooms;
    }
}
