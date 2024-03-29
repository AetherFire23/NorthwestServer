﻿using WebAPI.Enums;

namespace WebAPI.Entities;

public class Card
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CardImpact CardImpact { get; set; }


    // IsDrawn?
    public bool IsDiscarded { get; set; }

    public override string ToString()
    {
        return $"Card:{Name}";
    }
}
