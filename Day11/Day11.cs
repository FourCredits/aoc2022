using System.Text.RegularExpressions;

using AOC.Common;

var rx = TextNumber();
var monkeys = Parse(File.ReadAllLines("inputs/11.txt"));
var monkeys2 = monkeys.Select(m => new Monkey(m)).ToList();
Console.WriteLine($"part 1: {Part1(monkeys)}");  // 57838
Console.WriteLine($"part 2: {Part2(monkeys2)}"); // 15050382231

ulong Part1(List<Monkey> monkeys) =>
    Solve(20, (m, i) => m.Inspect(i) / 3, monkeys);

ulong Part2(List<Monkey> monkeys)
{
    // Thanks to https://chasingdings.com/2022/12/11/advent-of-code-day-11-monkey-in-the-middle/
    // for the idea of the modulo `% wrap` bit. I knew something like that was
    // involved, but I couldn't figure out what
    var wrap = monkeys.Select(m => m.TestNum).Aggregate((a, b) => a * b);
    return Solve(10_000, (m, i) => m.Inspect(i) % wrap, monkeys);
}

ulong Solve(
    int rounds,
    Func<Monkey, ulong, ulong> calcWorry,
    List<Monkey> monkeys)
{
    for (var round = 0; round < rounds; ++round)
    {
        foreach (var monkey in monkeys)
        {
            foreach (var item in monkey.Items)
            {
                var worry = calcWorry(monkey, item);
                var next = worry % monkey.TestNum == 0
                    ? monkey.IfTrue
                    : monkey.IfFalse;
                monkeys[next].Items.Add(worry);
            }
            monkey.ItemsThrown += (ulong)monkey.Items.Count;
            monkey.Items.Clear();
        }
    }
    return monkeys
        .Select(monkey => monkey.ItemsThrown)
        .Order()
        .TakeLast(2)
        .Aggregate((a, b) => a * b);
}

List<Monkey> Parse(IEnumerable<string> lines) =>
    lines.SplitBy(s => s != "").Select(ParseMonkey).ToList();

Monkey ParseMonkey(IEnumerable<string> lines)
{
    var source = lines.ToArray();
    return new(
        Number: TextNumberColon().Match(source[0]).Groups[1].Value.Read(),
        Items: TextThenNumberList()
            .Match(source[1])
            .Groups[2]
            .Captures
            .Select(n => (ulong)n.Value.Read())
            .ToList(),
        Inspect: DetermineInspectionFunction(source[2]),
        TestNum: (ulong)ExtractNum(source[3]),
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
    ulong TestNum,
    int IfTrue,
    int IfFalse)
{
    public ulong ItemsThrown { get; set; }

    public Monkey(Monkey m)
    {
        Number = m.Number;
        Items = new(m.Items);
        Inspect = m.Inspect;
        TestNum = m.TestNum;
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
