namespace AOC.Common;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> SplitBy<T>(
        this IEnumerable<T> input,
        Func<T, bool> pred)
    {
        List<IEnumerable<T>> result = new();
        List<T> current = new();
        foreach (var x in input)
            if (pred(x))
                current.Add(x);
            else if (current.Any())
            {
                result.Add(current);
                current = new();
            }
        return result;
    }
}
