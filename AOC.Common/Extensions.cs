using System.Globalization;

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
        if (current.Any())
            result.Add(current);
        return result;
    }

    public static IEnumerable<(int index, T item)> Enumerate<T>(
        this IEnumerable<T> input)
    {
        var i = 0;
        foreach (var t in input)
        {
            yield return (i, t);
            ++i;
        }
    }

    public static T2 GetValueOrInsert<T1, T2>(
        this IDictionary<T1, T2> dict,
        T1 key,
        Func<T2> valueGenerator)
    {
        if (!dict.ContainsKey(key)) dict[key] = valueGenerator();
        return dict[key];
    }

    // I got tired of the complaint about int.Parse missing a locale, so I've
    // made the decision to squash that error by putting the locale in one place
    public static int Read(this string s) =>
        int.Parse(s, new CultureInfo("en-NZ"));

    public static int Read(this Span<char> s) =>
        int.Parse(s, NumberStyles.None, new CultureInfo("en-NZ"));

    public static int Read(this ReadOnlySpan<char> s) =>
        int.Parse(s, NumberStyles.None, new CultureInfo("en-NZ"));

    public static IEnumerable<((int, int) pos, char c)> Grid(
        this IEnumerable<string> lines)
    {
        foreach (var (y, line) in lines.Enumerate())
            foreach (var (x, c) in line.Enumerate())
                yield return ((y, x), c);
    }

    public static IEnumerable<IEnumerable<T>> Windows<T>(
        this IEnumerable<T> iter,
        int windowSize,
        WindowMode mode = WindowMode.TrimPartials,
        int shiftAmount = 1) =>
        0.Iterate(i => i + shiftAmount)
            .Select(i => iter.Skip(i).Take(windowSize))
            .TakeWhile(
                mode == WindowMode.KeepPartials
                    ? window => window.Any()
                    : window => window.Count() == windowSize);

    public static IEnumerable<T> Iterate<T>(this T start, Func<T, T> f)
    {
        var x = start;
        while (true)
        {
            yield return x;
            x = f(x);
        }
    }

    public static IEnumerable<T> Cycle<T>(this IEnumerable<T> values)
    {
        while (true)
            foreach (var value in values)
                yield return value;
    }
}

public enum WindowMode
{
    KeepPartials,
    TrimPartials,
}
