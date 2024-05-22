using DomainTests;
using Newtonsoft.Json;

public static class NamesListReader
{
    private static List<string> _names = [];
    public static List<string> Read
    {
        get
        {
            if (!_names.Any())
            {
                _names = JsonConvert.DeserializeObject<List<string>>(Resource1.SerializedNames) ?? throw new Exception();
            }

            return _names;
        }
    }
}