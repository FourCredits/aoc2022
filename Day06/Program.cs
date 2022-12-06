var line = File.ReadAllLines("inputs/06.txt").Single();
// 1911 is too low
Console.WriteLine($"part 1: {FindNDistinctCharacters(line, 4)}");
Console.WriteLine($"part 2: {FindNDistinctCharacters(line, 14)}");

static int? FindNDistinctCharacters(string s, int n)
{
    for (var i = 0; i < s.Length; ++i)
        if (s[i..(i + n)].Distinct().Count() == n)
            return i + n;
    return null;
}
