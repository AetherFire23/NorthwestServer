﻿using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.GameTasks;
using Shared_Resources.GameTasks.Implementations_Unity;
using WebAPI.SSE;

namespace WebAPI.GameTasks.Implementations;

[GameTask(GameTaskCodes.CraftTask)]
public class KraftTaskExecutea : KraftTaskBase
{
    // should create ItemRepository 
    // to do something like .AddItem(ownerId, ItemTpype)
    private readonly PlayerContext _playerContext;
    private readonly IGameSSESender _gameSSESender;

    public KraftTaskExecutea(PlayerContext playerContext, IGameSSESender gameSSESender)
    {
        _playerContext = playerContext;
        _gameSSESender = gameSSESender;
    }

    public override async Task Execute(GameTaskContext context)
    {
        var item = new Item()
        {
            Id = Guid.NewGuid(),
            OwnerId = context.Player.Id,
            ItemType = ItemType.Hose,
        };

        await _playerContext.Items.AddAsync(item);
        await _playerContext.SaveChangesAsync();

        await _gameSSESender.SendItemChangedEvent(context.GameId);
    }
}
