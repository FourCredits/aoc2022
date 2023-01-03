using AOC.Common;

namespace Day20;

public static class WithKey
{
    public static long FindCoordinateSum(List<(int, long item)> items)
    {
        var indexOf0 = items.Enumerate().First(x => x.item.item == 0).index;
        return new[] {
            items[(indexOf0 + 1000) % items.Count].item,
            items[(indexOf0 + 2000) % items.Count].item,
            items[(indexOf0 + 3000) % items.Count].item
        }.Sum();
    }

    public static void Mix(
        List<(int, long)> items,
        IEnumerable<(int, long)> mixOrder)
    {
        foreach (var (startingIndex, item) in mixOrder)
        {
            var currentIndex = items.IndexOf((startingIndex, item));
            Shift(items, currentIndex, item);
        }
    }

    private static void Shift<T>(
        List<T> items,
        int startingIndex,
        long shiftAmount)
    {
        var element = items[startingIndex];
        items.RemoveAt(startingIndex);
        var index = Common.Index(startingIndex + shiftAmount, items.Count);
        items.Insert(index, element);
    }
}
