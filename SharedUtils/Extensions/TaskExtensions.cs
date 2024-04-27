namespace SharedUtils.Extensions;

public static class TaskExtensions
{
    public static async Task<List<T2>> SelectAll<T, T2>(this Task self, List<T> source, Func<T, Task<T2>> selection)
    {
        List<T2> elements = (await Task.WhenAll(source.Select(selection))).ToList();
        return elements;
    }
}
