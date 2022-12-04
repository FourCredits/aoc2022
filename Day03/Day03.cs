var lines = File.ReadAllLines("inputs/03.txt");
Console.WriteLine($"part 1: {Part1(lines)}");
Console.WriteLine($"part 2: {Part2(lines)}");

int Part1(string[] rucksacks) => rucksacks
    .Select(r => FindCommonElement(Bisect(r)))
    .Sum(Priority);

int Part2(string[] rucksacks) => rucksacks
    .Chunk(3)
    .Select(FindCommonElement)
    .Sum(Priority);

string[] Bisect(string s) =>
    new[] { s[..(s.Length / 2)], s[(s.Length / 2)..] };

char FindCommonElement(IEnumerable<IEnumerable<char>> colls) =>
    colls.Aggregate((a, b) => a.Intersect(b)).Single();

int Priority(char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
