using Day16;

var practice = args.Length > 0 && args[0] == "practice";
var lines = File.ReadAllLines(practice
    ? "inputs/practice.txt"
    : "inputs/16.txt");
var explorer = new Explorer(lines.Select(Parsing.Line));
var part1 = explorer.Solve();
Console.WriteLine($"part 1: {part1}"); // 1647
var part2 = explorer.SolveWithHelp();
Console.WriteLine($"part 2: {part2}"); // 2169
