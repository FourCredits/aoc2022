using AOC.Common;

var practice = false;
var lines =
    File.ReadAllLines(practice ? "inputs/practice.txt" : "inputs/15.txt");
var y = practice ? 10 : 2_000_000;
HashSet<(int, int)> beacons = new();
Dictionary<(int x, int y), int> sensors = new(lines
    .Select(line =>
        {
            var (sx, sy, bx, by) = ParseLine(line);
            var distance = Manhattan((sx, sy), (bx, by));
            _ = beacons.Add((bx, by));
            return new KeyValuePair<(int, int), int>((sx, sy), distance);
        }));
var minBound = sensors.Select(kv => kv.Key.x - kv.Value).Min();
var maxBound = sensors.Select(kv => kv.Key.x + kv.Value).Max();
var part1 = Enumerable
    .Range(minBound, maxBound - minBound + 1)
    .Select(x => (x, y))
    .Count(p => !beacons.Contains(p) &&
        !sensors.ContainsKey(p) &&
        sensors.Any(kv => Manhattan(kv.Key, p) <= kv.Value));
Console.WriteLine($"part 1: {part1}"); // 5564017

static int Manhattan((int x, int y) p1, (int x, int y) p2) =>
    int.Abs(p1.x - p2.x) + int.Abs(p1.y - p2.y);

static (int, int, int, int) ParseLine(string s) => Parse.ManyInts(s) switch
{
    [var i1, var i2, var i3, var i4] => (i1, i2, i3, i4),
    _ => throw new ArgumentException($"wrong number of ints"),
};
