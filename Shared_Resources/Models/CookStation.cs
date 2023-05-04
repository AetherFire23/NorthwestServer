using Newtonsoft.Json;
using Shared_Resources.Enums;

namespace Shared_Resources.Models
{
    public class CookStationProperties
    {
        public int MoneyMade { get; set; } = 0;
        public State State { get; set; } = State.Pristine;

        public string ToJSON()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
