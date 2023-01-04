using AOC.Common;

using Day23;

var input = File
    .ReadAllLines("inputs/23.txt")
    .Grid()
    .Where(pair => pair.c == '#')
    .Select(pair => new P2(pair.pos.Item2, pair.pos.Item1))
    .ToHashSet();
var state = new State(input, CardinalDirection.North);
var part1 = state
    .Iterate(s => s.PerformRound())
    .ElementAt(10)
    .CountEmptySpace();
Console.WriteLine($"part 1: {part1}"); // 3689
var part2 = 1;
for (; ; ++part2)
{
    var next = state.PerformRound();
    // something's changed; keep going
    if (next.Field.Except(state.Field).Any())
        state = next;
    else break;
}
Console.WriteLine($"part 2: {part2}"); // 3689
