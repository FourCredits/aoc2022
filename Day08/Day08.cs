using AOC.Common;

var lines = File.ReadAllLines("inputs/08.txt");
var width = lines[0].Length;
var breadth = lines.Length;
var map = new int[breadth, width];
foreach (var (y, line) in lines.Enumerate())
    foreach (var (x, c) in line.Enumerate())
        map[y, x] = c - '0';
var part1 = Enumerable.Range(0, breadth)
    .SelectMany(y => Enumerable.Range(0, width).Select(x => (y, x)))
    .Count(p => Visible(map, (breadth, width), p));
Console.WriteLine($"part 1 is {part1}");
var part2 = Enumerable.Range(0, breadth)
    .SelectMany(y => Enumerable.Range(0, width).Select(x => (y, x)))
    .Max(p => ScenicScore(map, (breadth, width), p));
Console.WriteLine($"part 2 is {part2}");

static int ScenicScore(int[,] map, (int, int) size, (int, int) position)
{
    var (breadth, width) = size;
    var (y, x) = position;
    var height = map[y, x];
    var northScore = 0;
    for (var i = y - 1; i >= 0; --i)
    {
        northScore++;
        if (map[i, x] >= height)
            break;
    }
    var southScore = 0;
    for (var i = y + 1; i < breadth; ++i)
    {
        southScore++;
        if (map[i, x] >= height)
            break;
    }
    var westScore = 0;
    for (var i = x - 1; i >= 0; --i)
    {
        westScore++;
        if (map[y, i] >= height)
            break;
    }
    var eastScore = 0;
    for (var i = x + 1; i < width; ++i)
    {
        eastScore++;
        if (map[y, i] >= height)
            break;
    }
    return northScore * southScore * eastScore * westScore;
}

static bool Visible(int[,] map, (int, int) size, (int, int) position)
{
    var (breadth, width) = size;
    var (y, x) = position;
    var height = map[y, x];
    return y == 0
    || x == 0
    || y == breadth - 1
    || x == width - 1
    || Enumerable.Range(0, y).All(i => map[i, x] < height)
    || Enumerable.Range(y + 1, breadth - y - 1).All(i => map[i, x] < height)
    || Enumerable.Range(0, x).All(i => map[y, i] < height)
    || Enumerable.Range(x + 1, width - x - 1).All(i => map[y, i] < height);
}
