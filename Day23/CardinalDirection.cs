using AOC.Common;

namespace Day23;

public enum CardinalDirection
{
    North,
    East,
    South,
    West,
}

public static class CardinalDirectionExtensions
{
    public static CardinalDirection Next(this CardinalDirection d) => d switch
    {
        CardinalDirection.North => CardinalDirection.South,
        CardinalDirection.South => CardinalDirection.West,
        CardinalDirection.West => CardinalDirection.East,
        CardinalDirection.East => CardinalDirection.North,
        _ => throw new ArgumentException("Not a cardinal direction."),
    };

    public static P2[] ToSearch(this CardinalDirection d) => d switch
    {
        CardinalDirection.North => new[] { NorthWest, North, NorthEast },
        CardinalDirection.East => new[] { NorthEast, East, SouthEast },
        CardinalDirection.South => new[] { SouthEast, South, SouthWest },
        CardinalDirection.West => new[] { SouthWest, West, NorthWest },
        _ => throw new ArgumentException("not a cardinal direction"),
    };

    private static readonly P2 NorthWest = new(-1, -1);
    private static readonly P2 North = new(0, -1);
    private static readonly P2 NorthEast = new(1, -1);
    private static readonly P2 East = new(1, 0);
    private static readonly P2 SouthEast = new(1, 1);
    private static readonly P2 South = new(0, 1);
    private static readonly P2 SouthWest = new(-1, 1);
    private static readonly P2 West = new(-1, 0);
}
