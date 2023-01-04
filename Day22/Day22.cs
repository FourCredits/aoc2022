using System.Text.RegularExpressions;

using AOC.Common;

using Day22;

var practice = args.Length > 0 && args[0] == "practice";
var lines = File
    .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/22.txt")
    .SplitBy(line => line != "");
Dictionary<P2, char> map = new(lines
    .ElementAt(0)
    .Grid()
    .Where(pair => pair.c != ' ')
    .Select(pair => new KeyValuePair<P2, char>(
        new P2(pair.pos.Item2, pair.pos.Item1),
        pair.c)));
var steps = lines.ElementAt(1).Single().Steps();
var start = new State(map.Keys.Min(), Direction.Right);
var part1 = steps.Aggregate(start, (acc, s) => acc.StepWrap(map, s)).Password();
Console.WriteLine($"part 1: {part1}"); // 88226
var part2 = steps.Aggregate(start, (acc, s) => acc.StepCube(map, s)).Password();
Console.WriteLine($"part 2: {part2}"); // 57305

public static partial class Program
{
    [GeneratedRegex("(\\d+|L|R)")]
    private static partial Regex Rx();

    private static readonly Regex Step = Rx();

    public static IEnumerable<string> Steps(this string s) =>
        Step.Matches(s).Select(match => match.Groups[0].Value);

    public static int Facing(this Direction d) => d switch
    {
        Direction.Right => 0,
        Direction.Down => 1,
        Direction.Left => 2,
        Direction.Up => 3,
        _ => throw new ArgumentException($"unrecognized direction {d}"),
    };
}
