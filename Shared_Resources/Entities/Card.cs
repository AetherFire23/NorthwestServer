using Shared_Resources.Enums;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public CardValue Value { get; set; }
        public bool IsDiscarded { get; set; }
    }
}
