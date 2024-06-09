namespace SharedUtils.Extensions;

public static class ListExtensions
{
    public static List<T> Shuffle<T>(this List<T> initialList)
    {
        var shuffled = new List<T>();
        while (initialList.Count != 0)
        {
            int randomNumber = Random.Shared.Next(0, initialList.Count);
            T? randomValue = initialList.ElementAt(randomNumber);
            shuffled.Add(randomValue);
            _ = initialList.Remove(randomValue);
        }

        return shuffled;
    }

    public static async Task<IEnumerable<T2>> SequentialSelectAsync<T, T2>(this IEnumerable<T> source, Func<T, Task<T2>> selection)
    {
        var elements = new List<T2>();

        foreach (T? element in source)
        {
            T2? result = await selection(element);
            elements.Add(result);
        }

        return elements;
    }
}
