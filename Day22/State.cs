using AOC.Common;

namespace Day22;

public record struct State(P2 P, Direction D)
{
    public long Password() => ((P.Y + 1) * 1000) + ((P.X + 1) * 4) + D.Facing();

    public State StepWrap(Dictionary<P2, char> map, string instruction) =>
        instruction == "L" ? (this with { D = D.TurnLeft() })
        : instruction == "R" ? (this with { D = D.TurnRight() })
        : MoveForwardWrap(map, instruction.Read());

    private State MoveForwardWrap(Dictionary<P2, char> map, int steps)
    {
        var newP = P;
        foreach (var _ in Enumerable.Range(0, steps))
        {
            var next = NextPositionWrap(map, newP);
            if (!map.TryGetValue(next, out var c) | c == '#')
                break;
            newP = next;
        }
        return this with { P = newP };
    }

    private P2 NextPositionWrap(Dictionary<P2, char> map, P2 p) =>
        map.ContainsKey(p + D)
            ? p + D
            : D switch
            {
                Direction.Right => map.Keys.Where(pos => pos.Y == p.Y).Min(),
                Direction.Down => map.Keys.Where(pos => pos.X == p.X).Min(),
                Direction.Left => map.Keys.Where(pos => pos.Y == p.Y).Max(),
                Direction.Up => map.Keys.Where(pos => pos.X == p.X).Max(),
                _ => throw new ArgumentException($"invalid direction {D}"),
            };

    public State StepCube(Dictionary<P2, char> map, string instruction) =>
        instruction == "L" ? (this with { D = D.TurnLeft() })
        : instruction == "R" ? (this with { D = D.TurnRight() })
        : MoveForwardCube(map, instruction.Read());

    private State MoveForwardCube(Dictionary<P2, char> map, int steps)
    {
        var newState = this;
        foreach (var _ in Enumerable.Range(0, steps))
        {
            var (x, y) = newState.P + newState.D;
            var next = newState.NextPositionCube(x, y);
            if (!map.TryGetValue(next.P, out var c) | c == '#')
                break;
            newState = next;
        }
        return newState;
    }

    private State NextPositionCube(long x, long y) =>
        (y < 0 && x is >= 50 and < 100 && D == Direction.Up)
            ? new(new(0, x + 100), Direction.Right)
        : (x < 0 && y is >= 150 and < 200 && D == Direction.Left)
            ? new(new(y - 100, 0), Direction.Down)
        : (y < 0 && x is >= 100 and < 150 && D == Direction.Up)
            ? new(new(x - 100, 199), Direction.Up)
        : (y >= 200 && x is >= 0 and < 50 && D == Direction.Down)
            ? new(new(x + 100, 0), Direction.Down)
        : (x >= 150 && y is >= 0 and < 50 && D == Direction.Right)
            ? new(new(99, 149 - y), Direction.Left)
        : (x == 100 && y is >= 100 and < 150 && D == Direction.Right)
            ? new(new(149, 149 - y), Direction.Left)
        : (y == 50 && x is >= 100 and < 150 && D == Direction.Down)
            ? new(new(99, x - 50), Direction.Left)
        : (x == 100 && y is >= 50 and < 100 && D == Direction.Right)
            ? new(new(y + 50, 49), Direction.Up)
        : (y == 150 && x is >= 50 and < 100 && D == Direction.Down)
            ? new(new(49, x + 100), Direction.Left)
        : (x == 50 && y is >= 150 and < 200 && D == Direction.Right)
            ? new(new(y - 100, 149), Direction.Up)
        : (y == 99 && x is >= 0 and < 50 && D == Direction.Up)
            ? new(new(50, x + 50), Direction.Right)
        : (x == 49 && y is >= 50 and < 100 && D == Direction.Left)
            ? new(new(y - 50, 100), Direction.Down)
        : (x == 49 && y is >= 0 and < 50 && D == Direction.Left)
            ? new(new(0, 149 - y), Direction.Right)
        : (x < 0 && y is >= 100 and < 150 && D == Direction.Left)
            ? new(new(50, 149 - y), Direction.Right)
        : this with { P = new(x, y) };
}
