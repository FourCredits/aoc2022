using Day02;

Dictionary<char, Result> resultParse = new()
{
    {'X', Result.Loss},
    {'Y', Result.Draw},
    {'Z', Result.Win},
};
Dictionary<char, Shape> shapeParse = new()
{
    {'A', Shape.Rock},
    {'B', Shape.Paper},
    {'C', Shape.Scissors},
    {'X', Shape.Rock},
    {'Y', Shape.Paper},
    {'Z', Shape.Scissors},
};
Dictionary<Shape, Shape> better = new()
{
    {Shape.Rock,     Shape.Paper},
    {Shape.Paper,    Shape.Scissors},
    {Shape.Scissors, Shape.Rock},
};
Dictionary<Shape, Shape> worse =
    new(better.Select(kv => new KeyValuePair<Shape, Shape>(kv.Value, kv.Key)));

var input = File.ReadAllLines("inputs/02.txt");
Console.WriteLine($"part 1: {Part1(input)}");
Console.WriteLine($"part 2: {Part2(input)}");

int Part1(string[] input) =>
    Parse(shapeParse, shapeParse, input).Select(DeterminePoints1).Sum();

int Part2(string[] input) =>
    Parse(shapeParse, resultParse, input).Select(DeterminePoints2).Sum();

int DeterminePoints1((Shape Theirs, Shape Ours) row) =>
    (int)row.Ours + (int)(row.Theirs == row.Ours ? Result.Draw
        : better[row.Theirs] == row.Ours ? Result.Win
        : Result.Loss);

int DeterminePoints2((Shape Theirs, Result Result) row) =>
    (int)row.Result + (int)(row.Result switch
    {
        Result.Draw => row.Theirs,
        Result.Loss => worse[row.Theirs],
        Result.Win => better[row.Theirs],
        _ => throw new ArgumentException($"row.Result is invalid"),
    });

IEnumerable<(A, B)> Parse<A, B>
    (Dictionary<char, A> col1, Dictionary<char, B> col2, string[] lines) =>
    lines.Select(line => (col1[line[0]], col2[line[2]]));
