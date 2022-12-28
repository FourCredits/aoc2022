using Day17;

var air = File
    .ReadAllLines("inputs/17.txt")
    .Where(line => !string.IsNullOrWhiteSpace(line))
    .ToArray()
    .First()
    .ToArray();

var part1 = Runner.Run(air, 2022);
Console.WriteLine($"part 1: {part1}"); // 3179

var part2 = Runner.Run(air, 1_000_000_000_000L);
Console.WriteLine($"part 2: {part2}"); // 1567723342929
