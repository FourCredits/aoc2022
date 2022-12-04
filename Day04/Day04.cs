var pairs = File.ReadAllLines("inputs/04.txt").Select(Parse);
var part1 = pairs.Count(pair => Contains(pair) || Contains(Flip(pair)));
Console.WriteLine($"part 1: {part1}");
var part2 = pairs.Count(pair => Overlap(pair) || Overlap(Flip(pair)));
Console.WriteLine($"part 2: {part2}");

(T2, T1) Flip<T1, T2>((T1 first, T2 second) pair) => (pair.second, pair.first);

bool Contains(((int low, int high) first, (int low, int high) second) pair) =>
    pair.first.low >= pair.second.low && pair.first.high <= pair.second.high;

bool Overlap(((int low, int high) first, (int low, int high) second) pair) =>
    pair.first.high >= pair.second.low && pair.first.low <= pair.second.high;

((int, int), (int, int)) Parse(string line) =>
    line.Split(new[] { '-', ',' }).Select(int.Parse).ToArray() switch
    {
        [var a, var b, var c, var d] => ((a, b), (c, d)),
        _ => throw new ArgumentException(line),
    };
