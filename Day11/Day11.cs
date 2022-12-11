using System.Text.RegularExpressions;

using AOC.Common;

var rx = TextNumber();
var monkeys = Parse(File.ReadAllLines("inputs/practice.txt"));
// var monkeys = Parse(File.ReadAllLines("inputs/11.txt"));
var monkeys2 = monkeys.Select(m => new Monkey(m)).ToList();
// 57838
var part1 = Part1(monkeys);
Console.WriteLine($"part 1: {part1}");
// 1572154518 is too low
var part2 = Part2(monkeys2);
Console.WriteLine($"part 2: {part2}");

ulong Solve(int rounds, Func<Monkey, ulong, ulong> worryCalc, List<Monkey> monkeys)
{
    for (var round = 0; round < rounds; ++round)
    {
        foreach (var monkey in monkeys)
        {
            foreach (var item in monkey.Items)
            {
                monkey.ItemsThrown++;
                var worry = worryCalc(monkey, item);
                var next = monkey.Test(worry) ? monkey.IfTrue : monkey.IfFalse;
                monkeys[next].Items.Add(worry);
            }
            monkey.Items.Clear();
        }
    }
    return monkeys
        .Select(monkey => monkey.ItemsThrown)
        .Order()
        .TakeLast(2)
        .Aggregate((a, b) => a * b);
}

ulong Part1(List<Monkey> monkeys) =>
    Solve(20, (m, i) => m.Inspect(i) / 3, monkeys);

ulong Part2(List<Monkey> monkeys) =>
    Solve(10_000, (m, i) => m.Inspect(i), monkeys);

List<Monkey> Parse(IEnumerable<string> lines) =>
    lines.SplitBy(s => s != "").Select(ParseMonkey).ToList();

Monkey ParseMonkey(IEnumerable<string> lines)
{
    var source = lines.ToArray();
    var testNum = ExtractNum(source[3]);
    return new(
        Number: TextNumberColon().Match(source[0]).Groups[1].Value.Read(),
        Items: TextThenNumberList()
            .Match(source[1])
            .Groups[2]
            .Captures
            .Select(n => (ulong)n.Value.Read())
            .ToList(),
        Inspect: DetermineInspectionFunction(source[2]),
        Test: worryLevel => worryLevel % (ulong)testNum == 0,
        IfTrue: ExtractNum(source[4]),
        IfFalse: ExtractNum(source[5]));
}

Func<ulong, ulong> DetermineInspectionFunction(string line)
{
    if (line.Any(char.IsDigit))
    {
        var inspectNum = (ulong)ExtractNum(line);
        return line.Contains('*')
            ? old => old * inspectNum
            : old => old + inspectNum;
    }
    else return line.Contains('*')
        ? old => old * old
        : old => old + old;
}

int ExtractNum(string line) => rx.Match(line).Groups[1].Value.Read();

public record class Monkey(
    int Number,
    List<ulong> Items,
    Func<ulong, ulong> Inspect,
    Func<ulong, bool> Test,
    int IfTrue,
    int IfFalse)
{
    public ulong ItemsThrown { get; set; }

    public Monkey(Monkey m)
    {
        Number = m.Number;
        Items = new(m.Items);
        Inspect = m.Inspect;
        Test = m.Test;
        IfTrue = m.IfTrue;
        IfFalse = m.IfFalse;
    }

}

public partial class Program
{
    [GeneratedRegex(@"[^\d]+(\d+):")]
    private static partial Regex TextNumberColon();

    [GeneratedRegex(@"[^\d]+(\d+)")]
    private static partial Regex TextNumber();

    [GeneratedRegex(@"Starting items: ((\d+)(, )?)+")]
    private static partial Regex TextThenNumberList();
}
