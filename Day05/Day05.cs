using AOC.Common;

var lines = File.ReadAllLines("inputs/05.txt");
var index = Array.IndexOf(lines, "");
var startingPositions = lines[..index].Reverse().ToArray();
var positions = startingPositions[0]
    .Enumerate()
    .Where(p => char.IsDigit(p.item))
    .ToDictionary(p => p.index, p => p.item);
var instructions = lines[(index + 1)..];
Dictionary<char, List<char>> stacks1 = new();
Dictionary<char, List<char>> stacks2 = new();
foreach (var row in startingPositions[1..])
    foreach (var (i, c) in row.Enumerate())
        if (char.IsUpper(c) && positions.TryGetValue(i, out var pos))
        {
            stacks1.GetValueOrInsert(pos, () => new()).Add(c);
            stacks2.GetValueOrInsert(pos, () => new()).Add(c);
        }
Console.WriteLine($"part 1: {Part1(stacks1, instructions)}");
Console.WriteLine($"part 2: {Part2(stacks2, instructions)}");

string PerformInstructions(
    Dictionary<char, List<char>> stacks,
    string[] instructions,
    Func<List<char>, int, IEnumerable<char>> pickUpBoxes)
{
    foreach (var instruction in instructions)
    {
        var words = instruction.Split(' ');
        var (n, from, to) = (words[1].Read(), words[3][0], words[5][0]);
        foreach (var val in pickUpBoxes(stacks[from], n)) stacks[to].Add(val);
        stacks[from].RemoveRange(stacks[from].Count - n, n);
    }
    return new string(stacks.Select(kvp => kvp.Value.Last()).ToArray());
}

string Part1(Dictionary<char, List<char>> s, string[] i) =>
    PerformInstructions(s, i, (stack, n) => stack.TakeLast(n).Reverse());

string Part2(Dictionary<char, List<char>> s, string[] i) =>
    PerformInstructions(s, i, (stack, n) => stack.TakeLast(n));
