using System.Text.RegularExpressions;

using AOC.Common;

namespace Day21;

// Do I want to make subclasses?
public partial class MonkeyTree
{
    public string Name { get; }
    public long? Number { get; }
    public MonkeyTree? Dependent1 { get; }
    public MonkeyTree? Dependent2 { get; }
    public Operation? Operation { get; }

    public static MonkeyTree Build(string[] lines)
    {
        var numberMonkey = NumberRx();
        var operationMonkey = OperationRx();
        Dictionary<string, long> numberMonkeys = new();
        Dictionary<string, OpInfo> operationMonkeys = new();
        foreach (var line in lines)
        {
            if (numberMonkey.IsMatch(line))
            {
                var match = numberMonkey.Match(line);
                numberMonkeys.Add(
                    match.Groups[1].Value, match.Groups[2].Value.Read());
            }
            else if (operationMonkey.IsMatch(line))
            {
                var match = operationMonkey.Match(line);
                operationMonkeys.Add(
                    match.Groups[1].Value,
                    new(
                        Operation.Parse(match.Groups[3].Value),
                        match.Groups[2].Value,
                        match.Groups[4].Value));
            }
            else
                throw new ArgumentException("failed parse");
        }
        return Build(numberMonkeys, operationMonkeys, "root");
    }

    public static MonkeyTree Build(
        IReadOnlyDictionary<string, long> numberMonkeys,
        IReadOnlyDictionary<string, OpInfo> operationMonkeys,
        string root) =>
        operationMonkeys.TryGetValue(root, out var info) ?
            new(root,
                Build(numberMonkeys, operationMonkeys, info.M1),
                Build(numberMonkeys, operationMonkeys, info.M2),
                info.Operation)
        : numberMonkeys.TryGetValue(root, out var number) ? (new(root, number))
        : throw new ArgumentException($"Don't know the monkey {root}");

    private MonkeyTree(
        string name,
        MonkeyTree dependent1,
        MonkeyTree dependent2,
        Operation operation)
    {
        Name = name;
        Dependent1 = dependent1;
        Dependent2 = dependent2;
        Operation = operation;
    }

    private MonkeyTree(string name, long number)
    {
        Name = name;
        Number = number;
    }

    public long Collapse() =>
        Number is long x
            ? x
            : Operation!.Call(
                Dependent1!.Collapse(),
                Dependent2!.Collapse());

    public bool Contains(string name) =>
        Name == name ||
            (Dependent1 is not null && Dependent1.Contains(name)) ||
            (Dependent2 is not null && Dependent2.Contains(name));

    private static readonly string Us = "humn";

    public long FindWhatToSay() =>
        Dependent1!.Contains(Us)
            ? Dependent1!.FillInBlank(Dependent2!.Collapse())
            : Dependent2!.FillInBlank(Dependent1!.Collapse());

    public long FillInBlank(long target)
    {
        if (Name == Us)
            return target;
        if (Number is not null)
            throw new ArgumentException($"something's gone wrong: {Name}");
        var (direction, toFillIn, toCollapse) = Dependent1!.Contains(Us)
            ? (Operand.Left, Dependent1, Dependent2)
            : (Operand.Right, Dependent2, Dependent1);
        var arg = toCollapse!.Collapse();
        var newTarget = Operation!.Invert(direction).Call(target, arg);
        return toFillIn!.FillInBlank(newTarget);
    }

    [GeneratedRegex("^([a-z]{4}): ([0-9]+)$")]
    public static partial Regex NumberRx();

    [GeneratedRegex(@"^([a-z]{4}): ([a-z]{4}) ([+*\/-]) ([a-z]{4})$")]
    public static partial Regex OperationRx();
}

public record struct OpInfo(Operation Operation, string M1, string M2);
