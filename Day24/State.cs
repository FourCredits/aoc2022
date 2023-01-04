using AOC.Common;

namespace Day24;

public record struct State(int Minutes, P2 Pos)
{
    public IEnumerable<State> Next(Valley v)
    {
        var (minutes, _) = this;
        return Pos.FourNeighbours()
            .Append(Pos)
            .Where(v.Safe)
            .Select(pos => new State(minutes + 1, pos));
    }
}
