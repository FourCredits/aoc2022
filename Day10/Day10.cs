using System.Text;

using AOC.Common;

var lines = File.ReadAllLines("inputs/10.txt");
var iter = RunProgram(lines);
// 15020
var part1 = (new[] { 20, 60, 100, 140, 180, 220 })
    .Sum(i => i * iter.ElementAt(i - 1));
Console.WriteLine($"part 1: {part1}");
StringBuilder s = new();
foreach (var (i, x) in iter.Enumerate())
{
    _ = s.Append((x - (i % 40)) switch
    {
        -1 or 0 or 1 => '#',
        _ => '.'
    });
    if (((i + 1) % 40) == 0)
        _ = s.Append('\n');
}
// EFUGLPAP
var part2 = s.ToString();
Console.WriteLine($"part 2:\n{part2}");

static IEnumerable<int> RunProgram(string[] instructions)
{
    var x = 1;
    foreach (var instruction in instructions)
    {
        var words = instruction.Split(' ');
        if (instruction == "noop")
        {
            yield return x;
        }
        else if (words[0] == "addx" &&
                int.TryParse(instruction.Split(' ')[1], out var n))
        {
            yield return x;
            yield return x;
            x += n;
        }
    }
}
