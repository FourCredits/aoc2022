namespace AOC.Common;

public record struct P2(long X, long Y)
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
}
