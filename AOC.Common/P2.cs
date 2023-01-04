namespace AOC.Common;

public record struct P2(long X, long Y) : IComparable
{
    public static P2 operator +(P2 p1, P2 p2) =>
        new(p1.X + p2.X, p1.Y + p2.Y);

    public static P2 operator -(P2 p1, P2 p2) =>
        new(p1.X - p2.X, p1.Y - p2.Y);

    public static P2 operator *(P2 vector, int scalar) =>
        new(vector.X * scalar, vector.Y * scalar);

    public static long Manhattan(P2 p1, P2 p2) =>
        long.Abs(p1.X - p2.X) + long.Abs(p1.Y - p2.Y);

    public static IEnumerable<P2> ReverseManhattan(P2 point, int distance) =>
        Enumerable.Range(0, distance).SelectMany(i => new[]
            {
                point + (new P2(1, -1) * i) + new P2(distance, 0),
                point + (new P2(-1, 1) * i) + new P2(-distance, 0),
                point + (new P2(-1, -1) * i) + new P2(0, distance),
                point + (new P2(1, 1) * i) + new P2(0, -distance),
            });

    public bool InBounds((P2 topLeft, P2 bottomRight) bounds) =>
        X >= bounds.topLeft.X &&
        X <= bounds.bottomRight.X &&
        Y >= bounds.topLeft.Y &&
        Y <= bounds.bottomRight.Y;

    public int CompareTo(object? obj) => CompareTo(obj as P2?);

    public int CompareTo(P2? other) =>
        other is not P2 o ? 1
        : this == o ? 0
        : Y.CompareTo(o.Y) != 0 ? Y.CompareTo(o.Y)
        : X.CompareTo(o.X);

    public static bool operator <(P2 left, P2 right) =>
        left.CompareTo(right) < 0;

    public static bool operator <=(P2 left, P2 right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >(P2 left, P2 right) =>
        left.CompareTo(right) > 0;

    public static bool operator >=(P2 left, P2 right) =>
        left.CompareTo(right) >= 0;

    public static implicit operator P2(Direction d) => d switch
    {
        Direction.Right => new P2(1, 0),
        Direction.Down => new P2(0, 1),
        Direction.Left => new P2(-1, 0),
        Direction.Up => new P2(0, -1),
        _ => throw new NotImplementedException("unreachable"),
    };

    public IEnumerable<P2> EightNeighbours() => new P2[]
    {
        new(X, Y - 1),
        new(X + 1, Y - 1),
        new(X + 1, Y),
        new(X + 1, Y + 1),
        new(X, Y + 1),
        new(X - 1, Y + 1),
        new(X - 1, Y),
        new(X - 1, Y - 1),
    };
}
