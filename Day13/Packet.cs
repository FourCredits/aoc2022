using System.Globalization;
using System.Text;

using AOC.Common;

namespace Day13;

public record struct Packet(
    int? Value,
    IReadOnlyList<Packet> Elements) : IComparable
{
    public static Packet? Parse(ReadOnlySpan<char> s) =>
        Parse(s, 0) is var (data, _) ? data : null;

    private static (Packet, int)? Parse(ReadOnlySpan<char> s, int cursor)
    {
        if (s[cursor..].IsEmpty)
            return null;
        if (s[cursor] == '[')
        {
            var elements = new List<Packet>();
            var c = cursor + 1;
            while (s[c] != ']')
            {
                var result = Parse(s, c);
                if (result is null) return null;
                (var element, c) = result.Value;
                elements.Add(element);
                if (s[c] == ',') c++;
            }
            return (new(null, elements), c + 1);
        }
        else
        {
            var end = s[cursor..].IndexOfAny(new[] { ',', ']' });
            var value = end == -1
                ? s.Read()
                : s[cursor..(cursor + end)].Read();
            return (new Packet(value, new List<Packet>()), cursor + end);
        }
    }

    public override string ToString() => Value is int value
        ? value.ToString(new CultureInfo("en-NZ"))
        : new StringBuilder()
            .Append('[')
            .AppendJoin(',', Elements.Select(e => e.ToString()))
            .Append(']')
            .ToString();

    public bool IsNumber() => Value is not null;

    public bool IsList() => Value is null;

    private static Packet Singleton(Packet value) =>
        new(null, new List<Packet>() { value });

    public static bool operator <(Packet left, Packet right) =>
        (left.Value is not null, right.Value is not null) switch
        {
            (true, true) => left.Value < right.Value,
            (false, false) => CompareLists(left, right),
            (true, false) => Singleton(left) < right,
            _ => left < Singleton(right),
        };

    private static bool CompareLists(Packet left, Packet right)
    {
        if (left.Elements.Count == 0 && right.Elements.Count == 0)
        {
            return false;
        }
        if (left.Elements.Count == 0)
        {
            return true;
        }
        if (right.Elements.Count == 0)
        {
            return false;
        }
        for (var i = 0; i < int.Min(left.Elements.Count, right.Elements.Count); ++i)
        {
            if (left.Elements[i] < right.Elements[i])
            {
                return true;
            }
            if (left.Elements[i] > right.Elements[i])
            {
                return false;
            }
        }
        return left.Elements.Count < right.Elements.Count;
    }

    public int CompareTo(object? obj)
    {
        var right = obj as Packet?;
        return right is null || this < right ? -1 :
            this == right ? 0 :
            1;
    }

    public static bool operator >(Packet left, Packet right) =>
        left != right && right < left;

    public static bool operator >=(Packet left, Packet right) =>
        left == right || right < left;

    public static bool operator <=(Packet left, Packet right) =>
        left == right || left < right;
}
