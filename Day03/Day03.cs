var lines = File.ReadAllLines("inputs/03.txt");
Console.WriteLine($"part 1: {Part1(lines)}");
Console.WriteLine($"part 2: {Part2(lines)}");

static int Part1(string[] rucksacks) => rucksacks
    .SelectMany(r => r.Take(r.Length / 2).Intersect(r.Skip(r.Length / 2)))
    .Select(Priority)
    .Sum();

static int Part2(string[] rucksacks) => rucksacks
    .Chunk(3)
    .Select(g => Priority(Intersection(g).First()))
    .Sum();

static IEnumerable<char> Intersection(IEnumerable<IEnumerable<char>> colls) =>
    colls.Aggregate((a, b) => a.Intersect(b));

static int Priority(char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
