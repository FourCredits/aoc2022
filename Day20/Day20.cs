var practice = args.Length > 0 && args[0] == "practice";
var input = File
    .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/20.txt")
    .Select(int.Parse)
    .ToArray();
List<int> numbers = new(input);
foreach (var number in input)
{
    var index = numbers.IndexOf(number);
    if (number >= 0)
    {
        Console.WriteLine(number);
        Shift(numbers, index, number);
    }
    else
    {
        Console.WriteLine(int.Abs(number));
        ShiftBackwards(numbers, index, int.Abs(number));
    }
}
var indexOf0 = numbers.IndexOf(0);
var groveCoords = new[] {
    numbers[(indexOf0 + 1000) % numbers.Count],
    numbers[(indexOf0 + 2000) % numbers.Count],
    numbers[(indexOf0 + 3000) % numbers.Count]
};
foreach (var gc in groveCoords) Console.WriteLine(gc);
var part1 = groveCoords.Sum();
Console.WriteLine($"part 1: {part1}"); // not -11581, 8915 is too low

static void Shift<T>(List<T> items, int startingIndex, int shiftAmount)
{
    var element = items[startingIndex];
    for (var i = startingIndex; i < startingIndex + shiftAmount; ++i)
        items[Index(i, items.Count)] = items[Index(i + 1, items.Count)];
    items[Index(startingIndex + shiftAmount, items.Count)] = element;
}

static void ShiftBackwards<T>(List<T> items, int startingIndex, int shiftAmount)
{
    var element = items[startingIndex];
    for (var i = startingIndex; i > startingIndex - shiftAmount; --i)
        items[Index(i, items.Count)] = items[Index(i - 1, items.Count)];
    items[Index(startingIndex - shiftAmount, items.Count)] = element;
}

static int Index(int index, int size) =>
    index >= size ? Index(index - size, size)
    : index < 0 ? Index(index + size, size)
    : index;
