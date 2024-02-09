﻿using Shared_Resources.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.GameTasks.CheckListsBuilding;

public class CheckListsBuilder
{
    public List<PromptInfo> CheckLists = new List<PromptInfo>();

    public PromptInfo CreateCheckListPrompt<T>(List<T> objects, string description) where T : ITaskParameter
    {
        PromptInfo info = new PromptInfo(objects.Select(x => x as ITaskParameter).ToList(), description);
        CheckLists.Add(info);
        return info;
    }
}
