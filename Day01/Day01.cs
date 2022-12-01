global using Input = System.Collections.Generic.IEnumerable<int>;

using AOC.Common;

var input = Parse(File.ReadAllLines("inputs/01.txt"));
Console.WriteLine($"part 1: {Part1(input)}");
Console.WriteLine($"part 2: {Part2(input)}");

static Input Parse(string[] lines) => lines
    .SplitBy(line => line != "")
    .Select(ls => ls.Select(int.Parse).Sum());

static int Part1(Input counts) => counts.Max();

static int Part2(Input counts)
{
    var copy = counts.ToList();
    copy.Sort((e1, e2) => e2.CompareTo(e1));
    return copy.Take(3).Sum();
}
