namespace Northwest.Extensions;
public static class EnumFunExtensions
{
    //public static List<T> GetAllEnumValues<T>() where T : Enum
    //{
    //    return new List<T>(Enum.GetValues(typeof(T)) as T[]);
    //}

    public static List<T> GetAllEnumValues<T>() where T : Enum
    {
        return new List<T>(Enum.GetValues(typeof(T)) as T[]);
    }
}
