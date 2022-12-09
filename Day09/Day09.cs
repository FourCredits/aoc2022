using AOC.Common;

var lines = File.ReadAllLines("inputs/09.txt").Select(Parse);
Console.WriteLine($"part 1: {SimulateRope(2, lines)}");
Console.WriteLine($"part 2: {SimulateRope(10, lines)}");

static int SimulateRope(int numKnots, IEnumerable<((int, int), int)> moves)
{
    var knots = Enumerable.Repeat((0, 0), numKnots).ToArray();
    HashSet<(int, int)> visited = new();
    foreach (var (move, amount) in moves)
        foreach (var i in Enumerable.Range(0, amount))
        {
            knots[0] = Add(knots[0], move);
            for (var j = 1; j < numKnots; ++j)
                knots[j] = MoveNext(knots[j - 1], knots[j]);
            _ = visited.Add(knots.Last());
        }
    return visited.Count;
}

static (int, int) MoveNext((int y, int x) head, (int y, int x) next) =>
    DiagonallyAdjacent(head, next)
        ? next
        : Add(next, (Signum(head.y - next.y), Signum(head.x - next.x)));

static ((int, int), int) Parse(string s) =>
    (ParseDirection(s[0]), s[2..].Read());

static (int, int) ParseDirection(char c) => c switch
{
    'R' => (0, 1),
    'L' => (0, -1),
    'U' => (1, 0),
    'D' => (-1, 0),
    _ => throw new ArgumentException("unreachable."),
};

static int Signum(int x) => x > 0 ? 1 : x < 0 ? -1 : 0;

static bool DiagonallyAdjacent((int y, int x) head, (int y, int x) tail) =>
    int.Abs(head.y - tail.y) <= 1 && int.Abs(head.x - tail.x) <= 1;

static (int, int) Add((int y, int x) p1, (int x, int y) p2) =>
    (p1.y + p2.x, p1.x + p2.y);
