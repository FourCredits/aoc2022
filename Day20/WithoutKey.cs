using AOC.Common;

namespace Day20;

public static class WithoutKey
{
    public static int FindCoordinateSum(List<(int, int item)> items)
    {
        var indexOf0 = items.Enumerate().First(x => x.item.item == 0).index;
        return new[] {
            items[(indexOf0 + 1000) % items.Count].item,
            items[(indexOf0 + 2000) % items.Count].item,
            items[(indexOf0 + 3000) % items.Count].item
        }.Sum();
    }

    public static void Mix(
        List<(int, int)> items,
        IEnumerable<(int, int)> mixOrder)
    {
        foreach (var (startingIndex, item) in mixOrder)
        {
            var index = items.IndexOf((startingIndex, item));
            Shift(items, index, item);
        }
    }

    private static void Shift<T>(
        List<T> items,
        int startingIndex,
        int shiftAmount)
    {
        var element = items[startingIndex];
        items.RemoveAt(startingIndex);
        var index = Common.Index(startingIndex + shiftAmount, items.Count);
        items.Insert(index, element);
    }
}
