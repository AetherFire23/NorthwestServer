using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Constants.CheckListsBuilding;

public class PromptInfo
{
    public string Description { get; set; }
    public List<ITaskParameter> Targets { get; private set; } = new List<ITaskParameter>();

    // When ExactAmount
    public int ExactTargetAmount { get; private set; } = 0;

    // When it is a range of values
    public int MinimumSelection { get; private set; } = _defaultMinimumValue;
    public int MaximumSelectionAmount { get; private set; } = _defaultMaximumValue;
    public bool IsOptionalSelection { get; private set; }

    // For making checklist in Unity
    public int MinimumChecks => IsExactAmount ? ExactTargetAmount : MinimumSelection;
    public int MaximumChecks => IsExactAmount ? ExactTargetAmount : MaximumSelectionAmount;
    public bool IsExactAmount => ExactTargetAmount != 0;
    public bool IsMultipleChecks => MaximumSelectionAmount > 1 || ExactTargetAmount > 1;

    private const int _defaultMinimumValue = 1;
    private const int _defaultMaximumValue = 99;

    // chaine de memes noms 
    public PromptInfo(List<ITaskParameter> targets, string description)
    {
        Description = description;
        Targets = targets;
    }

    public PromptInfo SetMinimumTargetCount(int min)
    {
        if (ExactTargetAmount != 0) throw new Exception("Cannot set both exact amount and - or min-max value");

        MinimumSelection = min;
        return this;
    }

    public PromptInfo SetMaximumTargetCount(int max)
    {
        if (MinimumSelection > max) throw new Exception($"Minimum value cannot be higher than maximum");

        MaximumSelectionAmount = max;
        return this;
    }

    public PromptInfo SetExactAmount(int exactAmount)
    {
        if (MinimumSelection != _defaultMinimumValue || MaximumSelectionAmount != _defaultMaximumValue)
            throw new Exception("Cannot set both exact amount and - or min-max value");
        if (exactAmount.Equals(0))
            throw new Exception("Cannot set an exact amount of checks to 0");

        ExactTargetAmount = exactAmount;
        return this;
    }

    public PromptInfo SetOptional()
    {
        IsOptionalSelection = true;
        return this;
    }

    public List<object> GetPromptsAsObjects()
    {
        var objects = Targets.Cast<object>().ToList();
        return objects;
    }

    public void FilterFromLastPrompt(Func<bool, object> predicate)
    {
        // use this inside of NorthWest to configure conditionals. 
        // since taskBuilder inside unity has access to gamesate, can compare the objects to any state.
    }
}
