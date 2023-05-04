using Shared_Resources.Constants;
using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Resources.GameTasks
{
    public class SelectableTargetOptions // builder ?
    {
        public Dictionary<Type, List<object>> ValidTargets = new Dictionary<Type, List<object>>();
        public Dictionary<Type, int> MaximumSelectableTargets = new Dictionary<Type, int>();
        public List<Type> RequiredTargets { get; set; } = new List<Type>();

        public SelectableTargetOptions(List<object> targets)
        {
            ValidTargets = ConstTargetTypes.GetSortedTargetTypes();
            InitializeMaximumSelectableTargets();
        }

        private void InitializeMaximumSelectableTargets()
        {
            foreach (var target in ValidTargets)
            {
                this.MaximumSelectableTargets.Add(target.Key, 99);
            }
        }

        public void AddTarget<T>(T obj)
        {
            var t = typeof(T);
            ValidTargets.GetValueOrDefault(t).Add(obj);
        }

        public SelectableTargetOptions AddTargets<T>(List<T> objects)
        {
            var t = typeof(T);
            ValidTargets.GetValueOrDefault(t).AddRange(objects.Select(x => x as object).ToList());
            return this;
        }

        public void SetTargetTypeAsRequired(Type type)
        {
            RequiredTargets.Add(type);
        }

        public void SetMaximumTargetType(Type targetType, int maximum)
        {
            var currentValuye = MaximumSelectableTargets.GetValueOrDefault(targetType);

            currentValuye = maximum;
        }
    }
}
