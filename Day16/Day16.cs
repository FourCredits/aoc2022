using Day16;

var lines = File.ReadAllLines("inputs/16.txt");
var explorer = new Explorer(lines.Select(Parsing.Line));
var part1 = explorer.Solve();
Console.WriteLine($"part 1: {part1}"); // 1647
var part2 = explorer.SolveWithHelp();
Console.WriteLine($"part 2: {part2}"); // 2169
