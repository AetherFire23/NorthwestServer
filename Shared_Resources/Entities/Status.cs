using Shared_Resources.Enums;
using System;

namespace Shared_Resources.Entities
{
    public class Status
    {
        public Guid Id { get; set; }
        public StatusEffect Effect { get; set; }
        public string SerializedProperties = string.Empty;
    }
}
