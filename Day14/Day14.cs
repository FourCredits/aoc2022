using AOC.Common;

// var lines = File.ReadAllLines("inputs/practice.txt");
var lines = File.ReadAllLines("inputs/14.txt");
var occupied = new Dictionary<(int x, int y), Tile>();
foreach (var p in lines.Select(ParseLine).SelectMany(RealiseStructure))
{
    occupied[p] = Tile.Rock;
}
Console.WriteLine($"part 1: {Part1(occupied)}");
Console.WriteLine($"part 2: {Part2(occupied)}");

static int Part1(Dictionary<(int, int), Tile> start)
{
    Dictionary<(int x, int y), Tile> occupied = new(start);
    var bottom = occupied.Select(p => p.Key.y).Max();
    var sandStart = (500, 0);
    for (var i = 0; ; ++i)
    {
        (int, int y) sand = sandStart;
        while (true)
        {
            if (sand.y > bottom)
                return i;
            if (TryFindNextPos(occupied.ContainsKey, sand, out var next))
                sand = next;
            else
            {
                occupied.Add(sand, Tile.Sand);
                break;
            }
        }
    }
}

static int Part2(Dictionary<(int, int), Tile> start)
{
    Dictionary<(int x, int y), Tile> occupied = new(start);
    var bottom = occupied.Select(p => p.Key.y).Max() + 2;
    var sandStart = (500, 0);
    bool Contained((int, int y) pos) =>
        pos.y == bottom || occupied.ContainsKey(pos);
    for (var i = 0; ; ++i)
    {
        (int, int y) sand = sandStart;
        while (true)
        {
            if (sand.y > bottom)
                return i;
            if (TryFindNextPos(Contained, sand, out var next))
                sand = next;
            else if (sand == sandStart)
                return i + 1;
            else
            {
                occupied.Add(sand, Tile.Sand);
                break;
            }
        }
    }
}

static bool TryFindNextPos(
    Func<(int, int), bool> occupied,
    (int, int) pos,
    out (int, int) nextPos)
{
    foreach (var direction in new[] { (0, 1), (-1, 1), (1, 1) })
        if (!occupied(Add(pos, direction)))
        {
            nextPos = Add(pos, direction);
            return true;
        }
    nextPos = (0, 0);
    return false;
}

static List<(int, int)> ParseLine(string line)
{
    List<(int, int)> result = new();
    var words = line.Split(' ');
    for (var i = 0; i < words.Length; i += 2)
    {
        var coords = words[i].Split(",");
        result.Add((coords[0].Read(), coords[1].Read()));
    }
    return result;
}

static IEnumerable<(int x, int y)> RealiseStructure(List<(int x, int y)> points) =>
    points
        .Windows(2)
        .SelectMany(window =>
            RealiseLine(window.ElementAt(0), window.ElementAt(1)))
        .Prepend(points[0]);

static IEnumerable<(int, int)> RealiseLine((int x, int y) p1, (int x, int y) p2)
{
    var point = p1;
    var direction = (p1, p2) switch
    {
        var ((p1x, p1y), (p2x, p2y)) when p1x == p2x && p1y > p2y => (0, -1),
        var ((p1x, p1y), (p2x, p2y)) when p1x == p2x && p1y < p2y => (0, 1),
        var ((p1x, p1y), (p2x, p2y)) when p1y == p2y && p1x > p2x => (-1, 0),
        var ((p1x, p1y), (p2x, p2y)) when p1y == p2y && p1x < p2x => (1, 0),
        _ => throw new ArgumentException("malformed coordinate pair"),
    };
    List<(int, int)> result = new();
    while (point != p2)
    {
        point = Add(point, direction);
        result.Add(point);
    }
    return result;
}

static (int x, int y) Add((int x, int y) p1, (int x, int y) p2) =>
    (p1.x + p2.x, p1.y + p2.y);

public enum Tile
{
    Sand,
    Rock,
}
