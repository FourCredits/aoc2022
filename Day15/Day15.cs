using AOC.Common;

var lines = File.ReadAllLines("inputs/15.txt");
var y = 2_000_000;
HashSet<P2> beacons = new();
Dictionary<P2, long> sensors = new(lines
    .Select(line =>
        {
            var (sensor, beacon) = ParseLine(line);
            var distance = P2.Manhattan(sensor, beacon);
            _ = beacons.Add(beacon);
            return new KeyValuePair<P2, long>(sensor, distance);
        }));
var minBound = sensors.Select(kv => kv.Key.X - kv.Value).Min();
var maxBound = sensors.Select(kv => kv.Key.X + kv.Value).Max();
var part1 = Enumerable
    .Range((int)minBound, (int)(maxBound - minBound + 1))
    .Select(x => new P2(x, y))
    .Count(p => !beacons.Contains(p) &&
        !sensors.ContainsKey(p) &&
        sensors.Any(kv => P2.Manhattan(kv.Key, p) <= kv.Value));
Console.WriteLine($"part 1: {part1}"); // 5564017

var searchSize = 4_000_000;
var (distressX, distressY) = sensors
    .Select(kv => P2.ReverseManhattan(kv.Key, (int)kv.Value + 1))
    .Aggregate((s1, s2) => s1.Union(s2))
    .Single(p => p.InBounds((new(0, 0), new(searchSize, searchSize))) &&
        sensors.All(kv => P2.Manhattan(kv.Key, p) > kv.Value));
var part2 = ((ulong)distressX * (ulong)searchSize) + (ulong)distressY;
Console.WriteLine($"part 2: {part2}"); // 11558423398893

static (P2, P2) ParseLine(string s) => Parse.ManyInts(s) switch
{
    [var i1, var i2, var i3, var i4] => (new(i1, i2), new(i3, i4)),
    _ => throw new ArgumentException($"wrong number of ints"),
};
