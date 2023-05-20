using Shared_Resources.Constants;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class CookTaskBase : GameTaskBase
    {
        // could add reflection check to see if I have forgotten to set a taskCode/ provider
        public override GameTaskProvider Provider { get; } = GameTaskProvider.Room;
        public override GameTaskCodes Code { get; } = GameTaskCodes.Cook;

        public override bool Requires(GameState gameState)
        {
            string currentRoomName = gameState.Room.Name;
            bool isPlayerInCorrectRoom = currentRoomName.Equals("Kitchen1"); //|| currentRoomName.Equals("EntryHall");
            return isPlayerInCorrectRoom;
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            if (context.GameState.Room.Name != nameof(LevelTemplate.Kitchen1))
            {
                return new GameTaskValidationResult("");
            }

            return new GameTaskValidationResult();
        }

        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }

        // renvoie un dictionnaire parce que je me force a sorter les types 
        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            var targetPrompts = new CheckListsBuilder();
            List<DTOs.RoomDTO> rooms = gameState.Rooms;
            List<Player> players = gameState.Players;

            targetPrompts.CreateCheckListPrompt(rooms)
                .SetExactAmount(3);

            targetPrompts.CreateCheckListPrompt(players)
                .SetMinimumTargetCount(1)
                .SetMaximumTargetCount(1);

            var stations = gameState.Stations;
            targetPrompts.CreateCheckListPrompt(stations)
                .SetExactAmount(1);

            return targetPrompts;
        }

        public Dictionary<string, string> GetTaskParameters(GameState gameState)
        {
            // should really make a class to handle dictionary parameterization
            var station = gameState.Room.Stations.FirstOrDefault(x => x.GameTaskCode == GameTaskCodes.Cook);
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("stationName", station.Id.ToString());
            return dictionary;
            // faut un dictionnaire pour mapper les stations I guess ? En tk whatever
        }

        //public Dictionary<Type, SelectableTargetOptions> ProduceDictionary(List<object> allTargets)
        //{
        //    var dictionary = ConstTargetTypes.GetSortedTargetTypes();

        //    foreach (var target in allTargets)
        //    {
        //        var listOfKind = dictionary.GetValueOrDefault(target.GetType());

        //        if (listOfKind is null) throw new Exception($"Unkown target type : {target.GetType()}");

        //        listOfKind.Add(target);
        //    }

        //    return dictionary;
        //}

        //// faudrait-tu un auto-convert  ? 
        //public KeyValuePair<Type, List<object>> GetValuePair<T>(Type type, List<T> someList) where T : class
        //{
        //    var objList = someList.Select(x => x as object).ToList();
        //    var valuePair = new KeyValuePair<Type, List<object>>(type, objList);
        //    return valuePair;
        //}
    }
}
