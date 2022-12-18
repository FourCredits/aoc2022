using AOC.Common;

using Day13;

var test = args.Length > 0 && args[0] == "test";
var lines = File.ReadAllLines("inputs/13.txt")
    .Select(line => Packet.Parse(line));
var part1 = lines
    .SplitBy(packet => packet is not null)
    .Select((pair, i) => pair.ElementAt(0) <= pair.ElementAt(1) ? i + 1 : 0)
    .Sum();
Console.WriteLine($"part 1: {part1}"); // 5808
var marker1 = Packet.Parse("[[2]]");
var marker2 = Packet.Parse("[[6]]");
var sorted = lines
    .Where(packet => packet is not null)
    .Append(marker1)
    .Append(marker2)
    .Order()
    .ToList();
var part2 = (sorted.IndexOf(marker1) + 1) * (sorted.IndexOf(marker2) + 1);
Console.WriteLine($"part 2: {part2}");
