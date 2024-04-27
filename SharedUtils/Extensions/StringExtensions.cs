namespace SharedUtils.Extensions;

public static class StringExtensions
{
    public static T ToEnum<T>(this string str) where T : struct, Enum
    {
        if (!Enum.TryParse(str, out T enumValue))
        {
            throw new Exception($"Could not parse '{str}' into '{nameof(T)}'");
        }

        return enumValue;
    }
}