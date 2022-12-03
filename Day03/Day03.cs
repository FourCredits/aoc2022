var lines = File.ReadAllLines("inputs/03.txt");
var total = 0;
foreach (var line in lines)
{
    var length = line.Length;
    var firstCompartment = line.Take(length / 2);
    var secondCompartment = line.Skip(length / 2);
    var commonItems = firstCompartment.Intersect(secondCompartment);
    foreach (var commonItem in commonItems)
    {
        total += char.IsLower(commonItem)
            ? commonItem - 'a' + 1
            : commonItem - 'A' + 27;
    }
}
Console.WriteLine($"part 1: {total}");
var total2 = 0;
foreach (var group in lines.Chunk(3))
{
    var badge = group switch
    {
        [var a, var b, var c] => a.Intersect(b).Intersect(c).First(),
        _ => throw new IOException("this should be impossible"),
    };
    total2 += char.IsLower(badge)
        ? badge - 'a' + 1
        : badge - 'A' + 27;
}
Console.WriteLine($"part 2: {total2}");
