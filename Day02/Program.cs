using Input = System.Collections.Generic.IEnumerable<(char, char)>;

namespace Day02;

public static class Day02
{
    public static void Main()
    {
        var input = Parse(File.ReadAllLines("inputs/02.txt"));
        Console.WriteLine($"part 1: {Part1(input)}");
        Console.WriteLine($"part 2: {Part2(input)}");
    }

    public static int Part1(Input rounds) =>
        rounds.Select(r => Score1((new(r.Item1), new(r.Item2)))).Sum();

    public static int Part2(Input rounds) =>
        rounds.Select(r => Score2((new(r.Item1), new(r.Item2)))).Sum();

    public static int Score1((RPS Theirs, RPS Ours) round) =>
        (int)Play(round.Theirs, round.Ours) + (int)round.Ours;

    public static int Score2((RPS Theirs, GameResult Needed) r) =>
        (int)NeededShape(r.Theirs, r.Needed) + (int)r.Needed;

    public static RPS NeededShape(RPS theirs, GameResult result) =>
        result.IsLoss() ? theirs.Beats()
        : result.IsDraw() ? theirs
        : theirs.BeatenBy();

    public static GameResult Play(RPS theirs, RPS ours) =>
        theirs > ours ? GameResult.Loss
        : theirs == ours ? GameResult.Draw
        : GameResult.Win;

    public static Input Parse(string[] lines) => lines
        .Select(line => (line[0], line[2]));
}

public class RPS
{
    private readonly Shape _rps;

    public static readonly RPS Rock = new(Shape.Rock);
    public static readonly RPS Paper = new(Shape.Paper);
    public static readonly RPS Scissors = new(Shape.Scissors);

    private enum Shape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    public RPS(char c) => _rps = c switch
    {
        'A' or 'X' => Shape.Rock,
        'B' or 'Y' => Shape.Paper,
        'C' or 'Z' => Shape.Scissors,
        _ => throw new ArgumentException($"{nameof(c)} is invalid character")
    };

    private RPS(Shape s) => _rps = s;

    public static explicit operator int(RPS shape) => (int)shape._rps;

    public static bool operator >(RPS shape, RPS other) =>
        (shape._rps, other._rps) switch
        {
            (Shape.Rock, Shape.Scissors)
                or (Shape.Paper, Shape.Rock)
                or (Shape.Scissors, Shape.Paper) => true,
            _ => false,
        };

    public static bool operator <(RPS shape, RPS other) =>
        other == shape || other > shape;

    public RPS Beats() => new(_rps switch
    {
        Shape.Rock => Shape.Scissors,
        Shape.Paper => Shape.Rock,
        Shape.Scissors => Shape.Paper,
        _ => throw new ArgumentException("unreachable"),
    });

    public RPS BeatenBy() => new(_rps switch
    {
        Shape.Rock => Shape.Paper,
        Shape.Paper => Shape.Scissors,
        Shape.Scissors => Shape.Rock,
        _ => throw new ArgumentException("unreachable"),
    });

}

public struct GameResult
{
    private enum Result
    {
        Loss = 0,
        Draw = 3,
        Win = 6,
    }

    private readonly Result _r;

    public static readonly GameResult Loss = new(Result.Loss);
    public static readonly GameResult Draw = new(Result.Draw);
    public static readonly GameResult Win = new(Result.Win);

    public bool IsLoss() => _r == Result.Loss;

    public bool IsDraw() => _r == Result.Draw;

    public bool IsWin() => _r == Result.Win;

    public static explicit operator int(GameResult gr) => (int)gr._r;

    public GameResult(char c) => _r = c switch
    {
        'X' => Result.Loss,
        'Y' => Result.Draw,
        'Z' => Result.Win,
        _ => throw new ArgumentException($"{nameof(c)} is invalid character")
    };

    private GameResult(Result r) => _r = r;
}
