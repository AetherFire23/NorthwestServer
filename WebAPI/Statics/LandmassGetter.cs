using Newtonsoft.Json;
using Quartz.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.TestFolder;
using WebAPI.Utils;

namespace WebAPI.Statics
{
    public static class LandmassGetter
    {
        private static List<LandmassLayout> ReadLandmassLayouts()
        {
            var allLandmassesSerialized = File.ReadAllText("Layout.txt");
            List<LandmassLayout>? layouts = JsonConvert.DeserializeObject<List<LandmassLayout>>(allLandmassesSerialized) ?? new List<LandmassLayout>();
            return layouts;
        }

        private static LandmassLayout GetRandomLandmassLayout()
        {
            List<LandmassLayout> allLayouts = ReadLandmassLayouts();
            var randomIndex = RandomHelper.random.Next(0, allLayouts.Count);
            LandmassLayout randomLayout = allLayouts.ElementAt(randomIndex);
            return randomLayout;
        }

        public static LandmassLayout CreateNewLandmassLayoutAndInsertNames(List<string> drawnRoomNames)
        {
            LandmassLayout layout = GetRandomLandmassLayout();
            for (int i = 0; i < drawnRoomNames.Count; i++)
            {
                string name = drawnRoomNames.ElementAt(i);
                layout.AllRooms.ElementAt(i).Name = name;
            }

            return layout;
        }

        public static LandmassLayout CreateNewLandmass()
        {
            LandmassLayout layout = GetRandomLandmassLayout();
            return layout;
        }

        public static LandmassLayout InsertLandmassNamesInLayout(LandmassLayout layout, List<string> drawnRoomNames)
        {
            for (int i = 0; i < drawnRoomNames.Count; i++)
            {
                string name = drawnRoomNames.ElementAt(i);
                layout.AllRooms.ElementAt(i).Name = name;
            }

            return layout;
        }
    }
}