using AOC.Common;

namespace Day24;

public record struct Valley(
    HashSet<(P2 p, Direction d)> Blizzards,
    HashSet<P2> Walls)
{
    public Valley Tick()
    {
        // Print();
        var walls = Walls;
        return this with
        {
            Blizzards = Blizzards
                .Select(blizzard =>
                {
                    var p = blizzard.p + blizzard.d;
                    return (walls.Contains(p) ? Wrap(walls, p) + blizzard.d : p,
                            blizzard.d);
                })
                .ToHashSet()
        };
    }

    private void Print()
    {
        Thread.Sleep(500);
        Console.Clear();
        foreach (var pos in Walls)
        {
            Console.SetCursorPosition((int)pos.X, (int)pos.Y);
            Console.Write('#');
        }
        foreach (var (pos, c) in Blizzards)
        {
            Console.SetCursorPosition((int)pos.X, (int)pos.Y);
            Console.Write(c switch
            {
                Direction.Right => '>',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Up => '^',
                _ => throw new NotImplementedException(),
            });
        }
    }

    public bool Safe(P2 pos) =>
        pos.X >= 0 && pos.Y >= 0 &&
        !Walls.Contains(pos) && !Blizzards.Any(b => b.p == pos);

    private static P2 Wrap(HashSet<P2> walls, P2 pos)
    {
        var horizontal = walls.Where(p => p.X == pos.X && p != pos);
        return horizontal.Count() == 1
            ? horizontal.Single()
            : walls.Single(p => p.Y == pos.Y && p != pos);
    }
}
