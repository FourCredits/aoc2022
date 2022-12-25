namespace Day16;

public class Explorer
{
    private const string Start = "AA";
    private readonly Dictionary<string, string[]> _neighbours;
    private readonly Dictionary<string, int> _pressures;
    private readonly List<int> _possibleValues = new();

    public Explorer(IEnumerable<(string, int, string[])> rooms)
    {
        _neighbours = new();
        _pressures = new();
        foreach (var (room, flowRate, adjacents) in rooms)
        {
            _neighbours.Add(room, adjacents);
            _pressures.Add(room, flowRate);
        }
    }

    public Explorer(
        Dictionary<string, string[]> neighbours,
        Dictionary<string, int> pressures)
    {
        _neighbours = neighbours;
        _pressures = pressures;
    }

    public int Solve()
    {
        _possibleValues.Clear();
        FindSolutions(new(), Start, 30, 0, new());
        return _possibleValues.Max();
    }

    private void FindSolutions(
        HashSet<(string, int, int, HashSet<string>)> visited,
        string current,
        int timeLeft,
        int pressureRelieved,
        HashSet<string> opened)
    {
        if (!visited.Add((current, timeLeft, pressureRelieved, opened)))
            return;
        if (timeLeft == 0)
        {
            _possibleValues.Add(pressureRelieved);
            return;
        }
        void Recur(string next) => FindSolutions(
            visited,
            next,
            timeLeft - 1,
            pressureRelieved,
            opened);
        if (!opened.Contains(current))
        {
            _ = opened.Add(current);
            pressureRelieved += _pressures[current] * (timeLeft - 1);
            Recur(current);
            pressureRelieved -= _pressures[current] * (timeLeft - 1);
            _ = opened.Remove(current);
        }
        foreach (var neighbour in _neighbours[current])
            Recur(neighbour);
    }

    public int SolveWithHelp()
    {
        _possibleValues.Clear();
        FindSolutionsWithHelp(new(), Start, Start, 26, 0, new());
        return _possibleValues.Max();
    }

    private void FindSolutionsWithHelp(
        HashSet<(string, string, int, int, HashSet<string>)> visited,
        string you,
        string elephant,
        int timeLeft,
        int pressureRelieved,
        HashSet<string> opened)
    {
        var state1 = (you, elephant, timeLeft, pressureRelieved, opened);
        var state2 = (elephant, you, timeLeft, pressureRelieved, opened);
        if (!visited.Add(state1) ||
                (visited.Contains(state2) && you != elephant))
            return;
        if (timeLeft == 0)
        {
            _possibleValues.Add(pressureRelieved);
            return;
        }
        void WithValveOpened(string valve, Action action)
        {
            _ = opened.Add(valve);
            pressureRelieved += _pressures[valve] * (timeLeft - 1);
            action();
            pressureRelieved -= _pressures[valve] * (timeLeft - 1);
            _ = opened.Remove(valve);
        }
        void Recur(string you, string elephant) => FindSolutionsWithHelp(
            visited,
            you,
            elephant,
            timeLeft - 1,
            pressureRelieved,
            opened);
        void LetElephantExplore(string you)
        {
            if (!opened.Contains(elephant))
                WithValveOpened(elephant, () => Recur(you, elephant));
            foreach (var neighbour in _neighbours[elephant])
                Recur(you, neighbour);
        }
        if (!opened.Contains(you))
            WithValveOpened(you, () => LetElephantExplore(you));
        foreach (var neighbour in _neighbours[you])
            LetElephantExplore(neighbour);
    }
}
