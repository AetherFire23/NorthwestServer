using System.Collections.Generic;

namespace Shared_Resources.Interfaces
{
    public interface ITaskParameter
    {
        public KeyValuePair<string, string> GetKeyValuePairParameter(int index);
    }
}
