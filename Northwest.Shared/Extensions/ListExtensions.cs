namespace WebAPI.Extensions;

public static class ListExtensions
{
    public static List<T> Shuffle<T>(this List<T> initialList)
    {
        List<T> shuffled = new List<T>();
        while (initialList.Count != 0)
        {
            int randomNumber = Random.Shared.Next(0, initialList.Count);
            T? randomValue = initialList.ElementAt(randomNumber);
            shuffled.Add(randomValue);
            _ = initialList.Remove(randomValue);
        }

        return shuffled;
    }

    /// <summary> Each element in the list are matched based on the mutual indexes of the other list. requires equal list length</summary>
    public static void ApplyActionOnMutualIndexMatch<T1, T2>(this List<T1> list1, List<T2> list2, Action<T1, T2> action)
    {
        if (list1.Count != list2.Count)
            throw new ArgumentException($"The two lists provided in this method must be of equal length. List1: {list1.Count}, List2: {list2.Count}");

        for (int i = 0; i < list1.Count; i++)
        {
            T1? element1 = list1[i];
            T2? element2 = list2[i];

            action(element1, element2);
        }
    }

    public static async Task<List<T2>> SelectAsync<T, T2>(this List<T> source, Func<T, Task<T2>> selection)
    {
        List<T2> elements = new List<T2>();

        foreach (T? element in source)
        {
            T2? result = await selection(element);
            elements.Add(result);
        }

        return elements;
    }
}
