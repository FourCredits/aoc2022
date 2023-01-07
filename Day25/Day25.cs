using System.Diagnostics;

using AOC.Common;

var practice = args.Length > 0 && args[0] == "practice";
var part1 = File
    .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/25.txt")
    .Select(FromSnafu)
    .Sum()
    .ToSnafu();
Console.WriteLine($"part 1: {part1}"); // 20-==01-2-=1-2---1-0

static long FromSnafu(string num) =>
    num.Aggregate(0L, (acc, c) => (acc * 5) + c switch
    {
        '=' => -2,
        '-' => -1,
        '0' => 0,
        '1' => 1,
        '2' => 2,
        _ => throw new ArgumentException($"bad snafu format {num}"),
    });

public static partial class Program
{
    public static string ToSnafu(this long num) =>
        num == 0 ? "0" : new string(num.Unfold(Next).Reverse().ToArray());

    private static (char, long)? Next(long n) => (n, n % 5) switch
    {
        (0, _) => null,
        (_, 0) => ('0', n / 5),
        (_, 1) => ('1', n / 5),
        (_, 2) => ('2', n / 5),
        (_, 3) => ('=', (n + 2) / 5),
        (_, 4) => ('-', (n + 1) / 5),
        _ => throw new UnreachableException(),
    };
}
