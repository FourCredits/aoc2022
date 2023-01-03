namespace Day21;

public class Operation
{
    private readonly Func<long, long, long> _f;

    public static Operation Parse(string s) => s switch
    {
        "+" => Add,
        "-" => Subtract,
        "*" => Multiply,
        "/" => Divide,
        _ => throw new ArgumentException("unrecognized command"),
    };

    private Operation(Func<long, long, long> f) => _f = f;

    public long Call(long a, long b) => _f(a, b);

    public Operation Invert(Operand operand) => this switch
    {
        var o when o == Add => new((c, b) => c - b),
        var o when o == Multiply => new((c, b) => c / b),
        var o when o == Subtract && operand == Operand.Left =>
            new((c, b) => c + b),
        var o when o == Subtract && operand == Operand.Right =>
            new((c, a) => a - c),
        var o when o == Divide && operand == Operand.Left =>
            new((c, b) => c * b),
        var o when o == Divide && operand == Operand.Right =>
            new((c, a) => a / c),
        _ => throw new ArgumentException("can't invert this"),
    };

    public static readonly Operation Add = new((a, b) => a + b);
    public static readonly Operation Subtract = new((a, b) => a - b);
    public static readonly Operation Multiply = new((a, b) => a * b);
    public static readonly Operation Divide = new((a, b) => a / b);
}

public enum Operand
{
    Left,
    Right
}
