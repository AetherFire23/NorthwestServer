﻿using Shared_Resources.Entities;
using Shared_Resources.Models;

namespace WebAPI.Interfaces
{
    public interface IPlayerService
    {
        public void TransferItem(Guid ownerId, Guid targetId, Guid itemId);
        public void UpdatePosition(Player unityPlayerModel);
        public ClientCallResult ChangeRoom(Guid playerId, string targetRoomName);
    }
}