using AOC.Common;

namespace Day17;

public class TetrisState
{
    public HashSet<P2> Stones { get; }
    public Dictionary<Pattern, (long rockIndex, long top)> Patterns { get; }
    public long Top { get; set; }
    public int AirIndex { get; set; }
    public long PatternAdded { get; set; }
    public long RockIndex { get; set; }

    public TetrisState()
    {
        Stones = new()
        {
            new(0, 0),
            new(1, 0),
            new(2, 0),
            new(3, 0),
            new(4, 0),
            new(5, 0),
            new(6, 0),
        };
        Patterns = new();
    }

    public bool Collides(HashSet<P2> rock) => rock.Overlaps(Stones);

    public void NextAirIndex(char[] air) =>
        AirIndex = (AirIndex + 1) % air.Length;

    public void FinaliseRock(
        long rockCount,
        HashSet<P2> rock)
    {
        Stones.UnionWith(rock);
        Top = Math.Max(Top, rock.Max(c => c.Y));
        if (Top >= 15)
            CheckForLoop(rockCount);
    }

    private void CheckForLoop(long rockCount)
    {
        var pattern = GetPattern(15);
        if (Patterns.TryGetValue(pattern, out var result))
        {
            var distanceY = Top - result.top;
            var numRocks = RockIndex - result.rockIndex;
            var multiple = (rockCount - RockIndex) / numRocks;
            PatternAdded += distanceY * multiple;
            RockIndex += numRocks * multiple;
        }
        Patterns[pattern] = (RockIndex, Top);
    }

    private Pattern GetPattern(long maxHeight)
    {
        var maxY = Stones.Select(c => c.Y).Max();
        var patternStones = Stones
            .Where(c => maxY - c.Y < maxHeight)
            .Select(c => new P2(c.X, maxY - c.Y))
            .ToHashSet();
        return new Pattern(AirIndex, patternStones);
    }
}
