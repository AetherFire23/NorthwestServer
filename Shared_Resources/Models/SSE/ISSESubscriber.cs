using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Models.SSE
{
    public interface ISSESubscriber
    {
        public Guid Id { get; set; }
    }
}
