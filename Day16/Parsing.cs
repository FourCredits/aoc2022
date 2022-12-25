using System.Text.RegularExpressions;

using AOC.Common;

namespace Day16;

public static partial class Parsing
{
    [GeneratedRegex("Valve (?<ident>[A-Z]{2}) has flow rate=(?<flow_rate>\\d+); tunnels? leads? to valves? ((?<others>[A-Z]{2})(, )?)+")]
    private static partial Regex line();

    private static readonly Regex Matcher = line();

    public static (string room, int flowRate, string[] neighbours)
        Line(string s)
    {
        var groups = Matcher.Match(s).Groups;
        return (
            groups["ident"].Value,
            groups["flow_rate"].Value.Read(),
            groups["others"].Captures.Select(c => c.Value).ToArray()
        );
    }
}

