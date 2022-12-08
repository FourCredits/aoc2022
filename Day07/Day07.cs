using AOC.Common;

var lines = File.ReadAllLines("inputs/07.txt");
var cwd = "";
Dictionary<string, List<string>> directories = new();
Dictionary<string, int> fileSizes = new();
foreach (var line in lines)
{
    var words = line.Split(' ');
    if (words[0] == "$")
    {
        if (words[1] == "cd")
        {
            cwd = words[2] == ".."
                ? Path.GetDirectoryName(cwd) ?? ""
                : Path.Combine(cwd, words[2]);
            _ = directories.TryAdd(cwd, new());
        }
    }
    else
    {
        var fileName = Path.Combine(cwd, words[1]);
        directories[cwd].Add(fileName);
        if (words[0] != "dir")
            fileSizes.Add(fileName, words[0].Read());
    }
}
Dictionary<string, int> directorySizes = new(directories
    .Keys
    .Select(k => new KeyValuePair<string, int>(k, FileSize(k))));

var part1 = directorySizes.Values.Where(size => size <= 100_000).Sum();
Console.WriteLine($"part 1: {part1}");

var spaceNeeded = directorySizes["/"] - 40_000_000;
var part2 = directorySizes.Values.Where(size => size > spaceNeeded).Min();
Console.WriteLine($"part 2: {part2}");

int FileSize(string file) => fileSizes.TryGetValue(file, out var size)
    ? size
    : directories[file].Select(FileSize).Sum();
