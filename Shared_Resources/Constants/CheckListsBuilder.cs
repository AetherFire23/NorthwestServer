using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Resources.Constants
{
    public class CheckListsBuilder
    {
        public List<PromptInfo> CheckLists = new List<PromptInfo>();

        public PromptInfo CreateCheckListPrompt<T>(List<T> objects, string description) where T : ITaskParameter
        {
            var info = new PromptInfo(objects.Select(x => x as ITaskParameter).ToList(), description);
            CheckLists.Add(info);
            return info;
        }
    }
}
