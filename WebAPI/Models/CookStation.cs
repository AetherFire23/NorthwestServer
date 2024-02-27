using Newtonsoft.Json;
using WebAPI.Enums;

namespace WebAPI.Models;

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
