using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Interfaces
{
    public interface IPlayerEntity : IEntity
    {
        Guid GameId { get; }
    }
}
