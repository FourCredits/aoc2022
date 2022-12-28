using AOC.Common;

namespace Day17;

public class Pattern : IEquatable<Pattern>
{
    private static readonly IEqualityComparer<HashSet<P2>> SetComparer =
        HashSet<P2>.CreateSetComparer();

    public Pattern(int airIndex, HashSet<P2> stones)
    {
        AirIndex = airIndex;
        Stones = stones;
    }

    public int AirIndex { get; }

    public HashSet<P2> Stones { get; }

    public bool Equals(Pattern? other) =>
        other != null &&
            AirIndex == other.AirIndex &&
            SetComparer.Equals(Stones, other.Stones);

    public override bool Equals(object? obj) =>
        Equals(obj as Pattern);

    public override int GetHashCode() =>
        HashCode.Combine(AirIndex, SetComparer.GetHashCode(Stones));
}

