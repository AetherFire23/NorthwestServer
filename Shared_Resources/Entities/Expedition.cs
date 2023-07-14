using System;

namespace Shared_Resources.Entities;

public class Expedition
{
    public Guid Id { get; set; }


    public Guid GameId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid LeaderId { get; set; }

    public bool IsAvailableForCreation { get; set; }

    public bool IsCreated { get; set; }
}
