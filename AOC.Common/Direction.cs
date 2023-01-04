namespace AOC.Common;

public enum Direction
{
    Right,
    Down,
    Left,
    Up,
}

public static class DirectionExtensions
{
    public static Direction TurnLeft(this Direction d) => d switch
    {
        Direction.Right => Direction.Up,
        Direction.Up => Direction.Left,
        Direction.Left => Direction.Down,
        Direction.Down => Direction.Right,
        _ => throw new NotImplementedException(),
    };

    public static Direction TurnRight(this Direction d) => d switch
    {
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        Direction.Up => Direction.Right,
        _ => throw new NotImplementedException(),
    };
}
