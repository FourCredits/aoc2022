using Day21;

var lines = File.ReadAllLines("inputs/21.txt");
var tree = MonkeyTree.Build(lines);
var part1 = tree.Collapse();
Console.WriteLine($"part 1: {part1}");
var part2 = tree.FindWhatToSay();
Console.WriteLine($"part 2: {part2}");
