namespace Day20;

public static class Common
{
    public static int Index(int index, int size) => index < 0
        ? size + index - (index / size * size)
        : index % size;

    public static int Index(long index, long size) =>
        (int)LongIndex(index, size);

    private static long LongIndex(long index, long size) => index < 0
        ? size + index - (index / size * size)
        : index % size;
}
