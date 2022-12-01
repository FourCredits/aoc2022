namespace AOC.Common;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> SplitBy<T>(
        this IEnumerable<T> input,
        Func<T, bool> pred)
    {
        // List<IEnumerable<T>> result = new();
        // while (input.Any())
        // {
        //     result.Add(input.TakeWhile(x => !pred(x)));
        //     Console.WriteLine($"{result.Count}, {result.Last().Count()}");
        //     input = input.SkipWhile(x => !pred(x)).Skip(1);
        // }
        // return result;
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
