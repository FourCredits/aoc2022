using AOC.Common;

var (map, start, end) = Parse(File.ReadAllLines("inputs/12.txt"));
if (start is null || end is null) return;
var part1 = Search(map, start.Value, end.Value);
Console.WriteLine($"part 1: {part1}");
var part2 = map
    .Where(kv => kv.Value == 0)
    .Min(kv => Search(map, kv.Key, end.Value));
Console.WriteLine($"part 2: {part2}");

int? Search(Dictionary<(int, int), int> map, (int, int) start, (int, int) end)
{
    Queue<((int, int), int)> queue = new();
    queue.Enqueue((start, 0));
    HashSet<(int, int)> visited = new();
    while (queue.TryDequeue(out var state))
    {
        var (pos, distance) = state;
        if (pos == end) return distance;
        if (!visited.Add(pos)) continue;
        var height = map[pos];
        bool Valid((int, int) next) => map.TryGetValue(next, out var nextHeight)
            && (nextHeight - height) <= 1;
        foreach (var nextPos in TaxicabNeighbours(pos).Where(Valid))
            queue.Enqueue((nextPos, distance + 1));
    }
    return null;
}

(Dictionary<(int, int), int>, (int, int)?, (int, int)?) Parse(string[] lines)
{
    Dictionary<(int, int), int> map = new();
    (int, int)? start = null;
    (int, int)? end = null;
    foreach (var (pos, c) in lines.Grid())
    {
        map[pos] = Height(c);
        if (c == 'S') start = pos;
        if (c == 'E') end = pos;
    }
    return (map, start, end);
}

int Height(char c) => c switch
{
    'S' => 0,
    'E' => 25,
    _ when char.IsLower(c) => c - 'a',
    _ => throw new ArgumentException($"invalid height map {c}"),
};

IEnumerable<(int, int)> TaxicabNeighbours((int y, int x) pos) => new[]
{
    (pos.y - 1, pos.x),
    (pos.y + 1, pos.x),
    (pos.y, pos.x - 1),
    (pos.y, pos.x + 1),
};
