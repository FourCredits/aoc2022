using AOC.Common;

using Day13;

var test = args.Length > 0 && args[0] == "test";
// var lines = File.ReadAllLines("inputs/practice.txt");
var lines = File.ReadAllLines("inputs/13.txt");
var part1 = lines
    .Select(line => Packet.Parse(line))
    .SplitBy(packet => packet is not null)
    .Select((pair, i) => pair.ElementAt(0) <= pair.ElementAt(1) ? i + 1 : 0)
    .Sum();
Console.WriteLine($"part 1: {part1}"); // 5684 is too low, 5834 is too high
if (test)
{
    TestParse();
    TestOrdering();
}

void TestOrdering()
{
    var testCases = new[]
    {
        ("[1,1,3,1,1]", "[1,1,5,1,1]", true),
        ("[[1],[2,3,4]]", "[[1],4]", true),
        ("[9]", "[[8,7,6]]", false),
        ("[[4,4],4,4]", "[[4,4],4,4,4]", true),
        ("[7,7,7,7]", "[7,7,7]", false),
        ("[]", "[3]", true),
        ("[[[]]]", "[[]]", false),
        ("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", false),
    };
    foreach (var (l, r, expected) in testCases)
    {
        var result = Packet.Parse(l) <= Packet.Parse(r);
        if (result != expected) Console.WriteLine("oh no!");
        Console.WriteLine($"{l}, {r}, {result}");
    }
}

void TestParse()
{
    var inputs = new[]
    {
        "9",
        "908",
        "[9]",
        "[[9]]",
        "[9,8]",
        "[9,8,7]",
        "[]",
        "[9,8,[7,6,[]]"
    };
    foreach (var input in inputs)
        Console.WriteLine($"\"{input}\": {Packet.Parse(input)}");
}
