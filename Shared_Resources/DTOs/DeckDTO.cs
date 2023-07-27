using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
namespace Shared_Resources.DTOs;

public class DeckDTO
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public List<Card> Cards { get; set; }
}
