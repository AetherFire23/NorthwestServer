using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Interfaces
{
    public interface ITaskParameter
    {
        public KeyValuePair<string, string> GetKeyValuePairParameter(int index);
    }
}
