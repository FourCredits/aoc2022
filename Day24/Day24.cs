using AOC.Common;

using Day24;

var practice = args.Length > 0 && args[0] == "practice";
Dictionary<P2, char> input = new(File
    // .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/24.txt")
    .ReadAllLines("inputs/practice2.txt")
    .Grid()
    .Select(kv =>
        new KeyValuePair<P2, char>(new(kv.pos.Item2, kv.pos.Item1), kv.c)));
var blizzards = input
    .Where(kv => kv.Value is not '#' and not '.')
    .Select(kv => (kv.Key, kv.Value switch
    {
        '<' => Direction.Left,
        '>' => Direction.Right,
        'v' => Direction.Down,
        '^' => Direction.Up,
        _ => throw new NotImplementedException(),
    }))
    .ToHashSet();
var walls = input
    .Where(kv => kv.Value == '#')
    .Select(kv => kv.Key)
    .ToHashSet();
// var valley = new Valley(blizzards, walls);
// while (true)
//     valley = valley.Tick();
var valleys = new Valley(blizzards, walls).Iterate(v => v.Tick());
var start = input.Where(kv => kv.Value == '.').Min(kv => kv.Key);
var end = input.Where(kv => kv.Value == '.').Max(x => x.Key);
var part1 = NavigateValley(valleys, start, end);
Console.WriteLine($"part 1: {part1}");

static int NavigateValley(IEnumerable<Valley> valleys, P2 start, P2 end)
{
    var startState = new State(0, start);
    Queue<State> queue = new();
    queue.Enqueue(startState);
    HashSet<State> visited = new() { startState };
    while (queue.TryDequeue(out var state))
    {
        Console.WriteLine(state);
        if (state.Pos == end)
            return state.Minutes;
        var valley = valleys.ElementAt(state.Minutes + 1);
        foreach (var nextState in state.Next(valley))
            if (visited.Add(nextState))
                queue.Enqueue(nextState);
    }
    throw new ArgumentException("No path found.");
}
