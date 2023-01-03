using System.Text.RegularExpressions;

using AOC.Common;

namespace Day21;

// Do I want to make subclasses?
public abstract partial class MonkeyTree
{
    public string Name { get; }

    protected static readonly string Us = "humn";
    private static readonly Regex NumberMonkey = NumberRx();
    private static readonly Regex OperationMonkey = OperationRx();

    protected MonkeyTree(string name) => Name = name;

    public static MonkeyTree Build(string[] lines)
    {
        Dictionary<string, long> numberMonkeys = new();
        Dictionary<string, OpInfo> operationMonkeys = new();
        foreach (var line in lines)
        {
            if (TryParseNumber(line, out var name, out var number))
                numberMonkeys.Add(name, number);
            else if (TryParseOperation(line, out var opName, out var info))
                operationMonkeys.Add(opName, info);
            else throw new ArgumentException("failed parse");
        }
        return Build(numberMonkeys, operationMonkeys, "root");
    }

    public static MonkeyTree Build(
        IReadOnlyDictionary<string, long> numberMonkeys,
        IReadOnlyDictionary<string, OpInfo> operationMonkeys,
        string root) =>
        operationMonkeys.TryGetValue(root, out var info) ?
            new OperationMonkey(
                root,
                Build(numberMonkeys, operationMonkeys, info.M1),
                Build(numberMonkeys, operationMonkeys, info.M2),
                info.Operation)
        : numberMonkeys.TryGetValue(root, out var number) ?
            new NumberMonkey(root, number)
        : throw new ArgumentException($"Don't know the monkey {root}");

    private static bool TryParseNumber(
        string line,
        out string name,
        out long number)
    {
        if (!NumberMonkey.IsMatch(line))
        {
            name = "";
            number = -1;
            return false;
        }
        var match = NumberMonkey.Match(line);
        name = match.Groups[1].Value;
        number = match.Groups[2].Value.Read();
        return true;
    }

    private static bool TryParseOperation(
        string line,
        out string name,
        out OpInfo opinfo)
    {
        if (!OperationMonkey.IsMatch(line))
        {
            name = "";
            opinfo = new(Operation.Fail, "", "");
            return false;
        }
        var match = OperationMonkey.Match(line);
        name = match.Groups[1].Value;
        opinfo = new(
            Operation.Parse(match.Groups[3].Value),
            match.Groups[2].Value,
            match.Groups[4].Value);
        return true;
    }

    public abstract long Collapse();

    public abstract bool Contains(string name);

    public abstract long FindWhatToSay();

    public abstract long FillInBlank(long target);

    [GeneratedRegex("^([a-z]{4}): ([0-9]+)$")]
    private static partial Regex NumberRx();

    [GeneratedRegex(@"^([a-z]{4}): ([a-z]{4}) ([+*\/-]) ([a-z]{4})$")]
    private static partial Regex OperationRx();
}

internal class NumberMonkey : MonkeyTree
{
    public long Number { get; }

    internal NumberMonkey(string name, long number)
        : base(name) => Number = number;

    public override long Collapse() => Number;

    public override bool Contains(string name) => Name == name;

    public override long FindWhatToSay() =>
        throw new ArgumentException("yeah no, don't start here");

    public override long FillInBlank(long target) =>
        Name == Us
            ? target
            : throw new ArgumentException($"something's gone wrong: {Name}");
}

internal class OperationMonkey : MonkeyTree
{
    public MonkeyTree Dependent1 { get; }
    public MonkeyTree Dependent2 { get; }
    public Operation Operation { get; }

    internal OperationMonkey(
        string name,
        MonkeyTree dependent1,
        MonkeyTree dependent2,
        Operation operation) : base(name)
    {
        Dependent1 = dependent1;
        Dependent2 = dependent2;
        Operation = operation;
    }

    public override long Collapse() =>
        Operation.Call(Dependent1.Collapse(), Dependent2.Collapse());

    public override bool Contains(string name) =>
        Name == name ||
            (Dependent1 is not null && Dependent1.Contains(name)) ||
            (Dependent2 is not null && Dependent2.Contains(name));

    public override long FindWhatToSay() =>
        Dependent1.Contains(Us)
            ? Dependent1.FillInBlank(Dependent2.Collapse())
            : Dependent2.FillInBlank(Dependent1.Collapse());

    public override long FillInBlank(long target)
    {
        var (direction, toFillIn, toCollapse) = Dependent1!.Contains(Us)
            ? (Operand.Left, Dependent1, Dependent2)
            : (Operand.Right, Dependent2, Dependent1);
        var arg = toCollapse!.Collapse();
        var newTarget = Operation!.Invert(direction).Call(target, arg);
        return toFillIn!.FillInBlank(newTarget);
    }
}

public record struct OpInfo(Operation Operation, string M1, string M2);
