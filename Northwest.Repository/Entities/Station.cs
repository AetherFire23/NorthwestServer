namespace Northwest.Persistence.Entities;

public class Station
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerializedProperties { get; set; } = string.Empty;// code is in the gamestak to know in which class to convert this
    public bool IsLandmass { get; set; }
    public bool IsActive { get; set; }
    public string RoomName { get; set; } = string.Empty;

    public KeyValuePair<string, string> GetKeyValuePairParameter(int index)
    {
        string prefix = $"station{index}";
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(prefix, Id.ToString()); // maybe I could use name this might be easier.
        return kvp;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Name";
    }

    public override string ToString()
    {
        return ToString(null, System.Globalization.CultureInfo.CurrentCulture);
    }
}
