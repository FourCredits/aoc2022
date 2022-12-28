using AOC.Common;

namespace Day17;

public static class Runner
{
    private static readonly List<HashSet<P2>> Stones = new()
    {
        new() { new(0, 0), new(1, 0), new(2, 0), new(3, 0) },
        new() { new(1, 2), new(0, 1), new(1, 1), new(2, 1), new(1, 0) },
        new() { new(2, 2), new(2, 1), new(0, 0), new(1, 0), new(2, 0) },
        new() { new(0, 3), new(0, 2), new(0, 1), new(0, 0) },
        new() { new(0, 1), new(1, 1), new(0, 0), new(1, 0) }
    };

    private static HashSet<P2> GetRock(
        long rockIndex,
        long bottomY) =>
        Stones[(int)(rockIndex % 5)]
            .Select(c => c + new P2(2, bottomY))
            .ToHashSet();

    public static long Run(char[] air, long rockCount)
    {
        var state = new TetrisState();
        for (; state.RockIndex < rockCount; ++state.RockIndex)
            PlaceRock(air, rockCount, state);
        return state.Top + state.PatternAdded;
    }

    private static void PlaceRock(
        char[] air,
        long rockCount,
        TetrisState state)
    {
        var rock = GetRock(state.RockIndex, state.Top + 4);
        while (true)
        {
            var gasShifted = ShiftDueToGas(air[state.AirIndex], rock);
            if (!state.Collides(gasShifted))
                rock = gasShifted;
            state.NextAirIndex(air);
            var down = ShiftDown(rock);
            if (!state.Collides(down))
                rock = down;
            else
            {
                state.FinaliseRock(rockCount, rock);
                break;
            }
        }
    }

    private static HashSet<P2> ShiftDueToGas(char gas, HashSet<P2> rock) =>
        gas == '>' ? ShiftRight(rock) : ShiftLeft(rock);

    private static readonly P2 Left = new(-1, 0);
    private static readonly P2 Right = new(1, 0);
    private static readonly P2 Down = new(0, -1);

    private static HashSet<P2> ShiftDown(HashSet<P2> rock) =>
        Shift(Down, rock);

    private static HashSet<P2> ShiftLeft(HashSet<P2> rock) =>
        rock.Any(c => c.X == 0) ? rock : Shift(Left, rock);

    private static HashSet<P2> ShiftRight(HashSet<P2> rock) =>
        rock.Any(c => c.X == 6) ? rock : Shift(Right, rock);

    private static HashSet<P2> Shift(P2 direction, HashSet<P2> rock) =>
        rock.Select(c => c + direction).ToHashSet();
}

