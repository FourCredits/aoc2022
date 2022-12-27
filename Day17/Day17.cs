using AOC.Common;

var practice = args.Length > 0 && args[0] == "practice";
var moves = File
    .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/17.txt")
    .Single()
    .Cycle()
    .GetEnumerator();
var shapes = Shapes();
var currentHeight = -1;
HashSet<P2> placedRocks = new();
P2 left = new(-1, 0);
P2 right = new(1, 0);
P2 down = new(0, -1);
for (var n = 0; n < 2022 && shapes.MoveNext(); ++n)
{
    var shape = shapes
        .Current
        .Select(p => p + new P2(2, currentHeight + 4));
    for (; moves.MoveNext();)
    {
        var next = moves.Current switch
        {
            '<' => shape.Select(p => p + left),
            '>' => shape.Select(p => p + right),
            _ => throw new ArgumentException($"{moves.Current} is invalid"),
        };
        if (next.All(p => p.X is <= 6 and >= 0) && !placedRocks.Overlaps(next))
            shape = next;
        next = shape.Select(p => p + down);
        if (next.All(p => p.Y >= 0) && !placedRocks.Overlaps(next))
            shape = next;
        else
        {
            placedRocks.UnionWith(shape);
            currentHeight = Math.Max(currentHeight, shape.Max(p => p.Y));
            break;
        }
    }
}
var part1 = placedRocks.Max(p => p.Y) + 1;
Console.WriteLine($"part 1: {part1}"); // 3179

static IEnumerator<IEnumerable<P2>> Shapes()
{
    var horizontalBar = new P2[]
    {
        new(0, 0),
        new(1, 0),
        new(2, 0),
        new(3, 0),
    };
    var plus = new P2[]
    {
        new(1, 0),
        new(0, 1),
        new(1, 1),
        new(2, 1),
        new(1, 2),
    };
    var angle = new P2[]
    {
        new(0, 0),
        new(1, 0),
        new(2, 0),
        new(2, 1),
        new(2, 2),
    };
    var verticalBar = new P2[]
    {
        new(0, 0),
        new(0, 1),
        new(0, 2),
        new(0, 3),
    };
    var box = new P2[]
    {
        new(0, 0),
        new(0, 1),
        new(1, 0),
        new(1, 1),
    };
    return new[] { horizontalBar, plus, angle, verticalBar, box }
        .Cycle()
        .GetEnumerator();
}
