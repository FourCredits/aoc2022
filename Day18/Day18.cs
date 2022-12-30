using AOC.Common;

var practice = args.Length > 0 && args[0] == "practice";
var points = File
    .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/18.txt")
    .Select(Parse)
    .ToHashSet();
var part1 = points
    .SelectMany(point => point.Neighbours())
    .Count(point => !points.Contains(point));
Console.WriteLine($"part 1: {part1}");

var ((minX, minY, minZ), (maxX, maxY, maxZ)) = Extrema(points);
HashSet<P3> externalPoints = new();
// add top and bottom planes
for (var y = minY - 1; y <= maxY + 1; ++y)
{
    for (var z = minZ - 1; z <= maxZ + 1; ++z)
    {
        _ = externalPoints.Add(new(minX - 1, y, z));
        _ = externalPoints.Add(new(maxX + 1, y, z));
    }
}
// add front and back planes
for (var x = minZ - 1; x <= maxX + 1; ++x)
{
    for (var y = minY - 1; y <= maxY + 1; ++y)
    {
        _ = externalPoints.Add(new(x, y, minZ - 1));
        _ = externalPoints.Add(new(x, y, maxZ + 1));
    }
}
// add left and right planes
for (var x = minX - 1; x <= maxX + 1; ++x)
{
    for (var z = minZ - 1; z <= maxZ + 1; ++z)
    {
        _ = externalPoints.Add(new(x, minY - 1, z));
        _ = externalPoints.Add(new(x, maxY + 1, z));
    }
}
// progressively move inwards, until no more external cubes of air are left
var count = 0;
HashSet<P3> alreadyChecked = new();
while (count != externalPoints.Count)
{
    count = externalPoints.Count;
    var newPoints = externalPoints
        .Except(alreadyChecked)
        .SelectMany(point => point.Neighbours())
        .Where(p =>
            p.X >= minX && p.X <= maxX &&
            p.Y >= minY && p.Y <= maxY &&
            p.Z >= minZ && p.Z <= maxZ)
        .Except(externalPoints)
        .Except(points)
        .ToArray();
    alreadyChecked.UnionWith(externalPoints);
    externalPoints.UnionWith(newPoints);
}
var part2 = points
    .SelectMany(point => point.Neighbours())
    .Count(point => !points.Contains(point) && externalPoints.Contains(point));
Console.WriteLine($"part 2: {part2}"); // 2498

static (P3 topFrontLeft, P3 bottomBackRight) Extrema(IEnumerable<P3> points) =>
    (topFrontLeft: new P3(
        points.Min(p => p.X),
        points.Min(p => p.Y),
        points.Min(p => p.Z)),
     bottomBackRight: new P3(
        points.Max(p => p.X),
        points.Max(p => p.Y),
        points.Max(p => p.Z)));

static P3 Parse(string line) => line.Split(",") switch
{
    [var x, var y, var z] => new(x.Read(), y.Read(), z.Read()),
    _ => throw new ArgumentException($"{line} is not of the form `a,b,c`"),
};

public record struct P3(int X, int Y, int Z)
{
    public P3[] Neighbours() => new[]
    {
        new P3(X, Y, Z + 1),
        new P3(X, Y, Z - 1),
        new P3(X, Y + 1, Z),
        new P3(X, Y - 1, Z),
        new P3(X + 1, Y, Z),
        new P3(X - 1, Y, Z),
    };
}
