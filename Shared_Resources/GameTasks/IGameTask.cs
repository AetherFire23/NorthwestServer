using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks
{
    public interface IGameTask
    {
         GameTaskEmittorTypes EmittorType { get; set; }
        // public string FormatTaskName{get; set;}
         bool CanShow(GameState gameState);
         GameTaskValidationResult Validate(GameTaskContext context);
         void Execute(GameTaskContext context);
    }
}
