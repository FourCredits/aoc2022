using AOC.Common;

namespace Day23;

public record struct State(HashSet<P2> Field, CardinalDirection Direction)
{
    public State PerformRound()
    {
        var proposedMoves = Propose();
        var newField = Move(proposedMoves);
        return new(newField, Direction.Next());
    }

    public long CountEmptySpace()
    {
        var ((minX, minY), (maxX, maxY)) = Bounds();
        return ((maxX - minX + 1) * (maxY - minY + 1)) - Field.Count;
    }

    private (P2 bottomRight, P2 topLeft) Bounds() =>
        (new P2(Field.Min(p => p.X), Field.Min(p => p.Y)),
         new P2(Field.Max(p => p.X), Field.Max(p => p.Y)));

    private IReadOnlyDictionary<P2, HashSet<P2>> Propose()
    {
        Dictionary<P2, HashSet<P2>> proposedMoves = new();
        foreach (var elf in Field)
        {
            if (MoveToPropose(elf) is P2 p)
                _ = proposedMoves
                    .GetValueOrInsert(p, () => new())
                    .Add(elf);
        }
        return proposedMoves;
    }

    private HashSet<P2> Move(IReadOnlyDictionary<P2, HashSet<P2>> proposed)
    {
        var canMove = proposed.Where(kv => kv.Value.Count == 1);
        return new(canMove
            .Select(kv => kv.Key)
            .Union(Field.Except(canMove.SelectMany(kv => kv.Value))));
    }

    private P2? MoveToPropose(P2 elf)
    {
        var field = Field;
        return elf.EightNeighbours().All(p => !field.Contains(p))
            ? null
            : Direction
            .Iterate(d => d.Next())
            .Take(4)
            .Select(P2? (d) =>
            {
                var squares = d.ToSearch().Select(s => s + elf).ToArray();
                return squares.All(s => !field.Contains(s)) ? squares[1] : null;
            })
            .FirstOrDefault(p => p is not null);
    }
}
