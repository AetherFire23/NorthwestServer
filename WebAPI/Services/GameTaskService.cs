﻿using Microsoft.AspNetCore.Mvc;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using System;
using WebAPI.GameTasks;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class GameTaskService : IGameTaskService
    {
        private readonly IGameStateRepository _gameStateRepository;
        private readonly IServiceProvider _serviceProvider;


        public GameTaskService(IGameStateRepository gameStateRepository, IServiceProvider serviceProvider)
        {
            _gameStateRepository = gameStateRepository;
            _serviceProvider = serviceProvider;
        }

        public ClientCallResult ExecuteGameTask(Guid playerId, GameTaskCode taskCode, Dictionary<string, string> parameters)
        {
            var gameState = _gameStateRepository.GetPlayerGameState(playerId, null);
            if (gameState == null)
            {
                return ClientCallResult.Failure;
            }

            var context = new GameTaskContext
            {
                GameState = gameState,
                Parameters = parameters ?? new Dictionary<string, string>()
            };

            Type gameTaskType = GameTaskTypeSelector.GetGameTaskType(taskCode);
            var gameTask = _serviceProvider.GetService(gameTaskType) as IGameTask;

            var result = gameTask.Validate(context);
            if (!result.IsValid)
            {
                return ClientCallResult.Failure;
            }

            gameTask.Execute(context);
            return ClientCallResult.Success;
        }
    }
}