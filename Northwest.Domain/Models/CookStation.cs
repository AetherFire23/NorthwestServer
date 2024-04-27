using Newtonsoft.Json;
using Northwest.Domain.Enums;

namespace Northwest.Domain.Models;

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
