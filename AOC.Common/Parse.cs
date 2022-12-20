using System.Text.RegularExpressions;

namespace AOC.Common;

public static partial class Parse
{
    private static readonly Regex Number = Int();

    public static int[] ManyInts(string s) =>
        Number.Matches(s).Select(m => m.Value.Read()).ToArray();

    [GeneratedRegex("-?[0-9]+")]
    private static partial Regex Int();
}
