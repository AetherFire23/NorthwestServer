using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Resources.Constants
{
    // Just let the task validation break for now if the parameters are incorrect.
    // So PromptInfo is 
    public class PromptInfo
    {
        public string Description { get; set; }
        public List<ITaskParameter> PromptedObjects { get; private set; } = new List<ITaskParameter>();
        public int MaximumSelectionAmount { get; private set; } = _defaultMaximumValue;
        public int MinimumSelection { get; private set; } = _defaultMinimumValue;

        public int ExactTargetAmount { get; private set; } = 0;

        public bool IsOptionalSelection { get; private set; } // Action 1 is instant and action 2 is optional (necessarily)

        public bool IsExactAmount => ExactTargetAmount != 0;
        public bool IsMultipleChecks => MaximumSelectionAmount > 1 || ExactTargetAmount > 1;
        public int MaximumChecks => IsExactAmount ? ExactTargetAmount : MaximumSelectionAmount;
        public int MinimumChecks => IsExactAmount ? ExactTargetAmount : MinimumSelection;
        private const int _defaultMinimumValue = 1;
        private const int _defaultMaximumValue = 99;

        // use this if some prompts are conditional and therefore requires recursion (highly doubt this is logically necessary however)

        /// <summary>
        /// No configuration entails unlimited selections and makes the prompt required.
        /// </summary>
        /// <param name="targets"></param>
        public PromptInfo(List<ITaskParameter> targets, string description)
        {
            Description = description;
            PromptedObjects = targets;
        }

        public PromptInfo SetMinimumTargetCount(int min)
        {
            if (this.ExactTargetAmount != 0) throw new Exception("Cannot set both exact amount and - or min-max value");

            MinimumSelection = min;
            return this;
        }

        public PromptInfo SetMaximumTargetCount(int max)
        {
            if (this.MinimumSelection > max) throw new Exception($"Minimum value cannot be higher than maximum");

            MaximumSelectionAmount = max;
            return this;
        }

        /// <summary>
        /// Incompatible with SetMinimumTargetCount and SetMaximumTargetCount.
        /// </summary>
        public PromptInfo SetExactAmount(int exactAmount)
        {
            bool minimumOrMaximumAlreadySet = !MinimumSelection.Equals(_defaultMinimumValue)
                || !MaximumSelectionAmount.Equals(_defaultMaximumValue);

            if (minimumOrMaximumAlreadySet) throw new Exception("Cannot set both exact amount and - or min-max value");
            if (exactAmount.Equals(0)) throw new Exception("Cannot set an exact amount of checks to 0");



            this.ExactTargetAmount = exactAmount;
            return this;
        }

        public PromptInfo SetOptional()
        {
            this.IsOptionalSelection = true;
            return this;
        }

        public List<object> GetPromptsAsObjects()
        {
            var objects = this.PromptedObjects.Select(x => x as object).ToList();
            return objects;
        }

        public void FilterFromLastPrompt(Func<bool, object> predicate)
        {
            // use this inside of NorthWest to configure conditionals. 
            // since taskBuilder inside unity has access to gamesate, can compare the objects to any state.


        }
    }
}
