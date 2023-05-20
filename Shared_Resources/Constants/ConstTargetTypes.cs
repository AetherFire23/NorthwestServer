using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Constants
{
    public class ConstTargetTypes // desuet
    {
        private static Dictionary<Type, List<object>> TheDictionary = new Dictionary<Type, List<object>>();

        public static Type PlayerType { get; set; } = typeof(Player);
        public static Type RoomType { get; set; } = typeof(RoomDTO);
        public static Type ItemType { get; set; } = typeof(Item);
        public static Type SKillType { get; set; } = typeof(Enums.SkillEnum);


        public static Dictionary<Type, List<object>> GetSortedTargetTypes()
        {
            if (TheDictionary is null) throw new Exception($"Dont forget to initialize {nameof(TheDictionary)}");

            var dictionary = new Dictionary<Type, List<object>>();
            foreach (var type in TheDictionary)
            {
                dictionary.Add(type.Key, new List<object>()); // but object is important to be new list because else it will modify TheDictionary
            }
            return dictionary;
        }

        public static void RegisterTargetTypes() // will have to be initilized in Unity GameLauncher
        {
            var types = typeof(ConstTargetTypes)
                .GetProperties()
            .Where(x => x.PropertyType == typeof(Type))
            .Select(x => x.GetValue(null) as Type).ToList();

            types.ToList().ForEach(x => TheDictionary.Add(x, new List<object>()));
        }
    }
}
