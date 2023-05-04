using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Resources.GameTasks
{
    public class TaskTargetInfo
    {
        public Dictionary<TargetTypes, int> MaximumTargets { get; set; }

        // je vais juste full automatiser le checkLIst I think
        public List<Player> PlayerTargets => GetValidTargetsOf<Player>();
        public List<Room> RoomTargets => GetValidTargetsOf<Room>();
        public List<Item> ItemTargets => GetValidTargetsOf<Item>();
        public List<Skill> SkillTargets => GetValidTargetsOf<Skill>();
        // public List<Skill> SkillTargets => GetValidTargetsOf<Skill>(); // stations I guess
        private List<object> _validTargets { get; set; } = new List<object>();

        private static Dictionary<TargetTypes, List<object>> _targetTypesMap = new Dictionary<TargetTypes, List<object>>();

        public TaskTargetInfo(List<object> validTargets, Dictionary<TargetTypes, int> maximumTargets)
        {
            _validTargets = validTargets;
        }

        public List<T> GetValidTargetsOf<T>() where T : class
        {
            var targets = _validTargets
                .Where(x => x is T)
                .Select(x => x as T).ToList();

            return targets;
        }
    }
}
