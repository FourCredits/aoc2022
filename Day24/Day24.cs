﻿var data = File.ReadAllLines("inputs/24.txt").ToArray();

var maxX = data[0].Length - 1;
var maxY = data.Length - 1;

var blizzards = new List<(int x, int y, char direction)>();
for (var y = 1; y < maxY; ++y)
    for (var x = 1; x < maxX; ++x)
        if (data[y][x] != '.')
            blizzards.Add((x, y, data[y][x]));

var maxBoards = (maxX - 1) * (maxY - 1);
var occupiedSpotsByRound = new List<HashSet<(int x, int y)>>();
var currentBlizzards = blizzards;
for (var boardNum = 0; boardNum < maxBoards; ++boardNum)
{
    var newBlizzards = new List<(int x, int y, char direction)>();
    var occupiedSpots = new HashSet<(int x, int y)>();

    foreach (var blizzard in currentBlizzards)
    {
        _ = occupiedSpots.Add((blizzard.x, blizzard.y));

        var newBlizzard = blizzard;
        if (blizzard.direction == '<')
        {
            if (--newBlizzard.x < 1)
                newBlizzard.x = maxX - 1;
        }
        else if (blizzard.direction == 'v')
        {
            if (++newBlizzard.y > maxY - 1)
                newBlizzard.y = 1;
        }
        else if (blizzard.direction == '>')
        {
            if (++newBlizzard.x > maxX - 1)
                newBlizzard.x = 1;
        }
        else if (blizzard.direction == '^')
        {
            if (--newBlizzard.y < 1)
                newBlizzard.y = maxY - 1;
        }

        newBlizzards.Add(newBlizzard);
    }

    occupiedSpotsByRound.Add(occupiedSpots);
    currentBlizzards = newBlizzards;
}

var visited = new HashSet<(int x, int y, int step, short stage)>();
var toRun = new Queue<(int x, int y, int step, short stage)>();
toRun.Enqueue((1, 0, 0, 0));
var foundPart1 = false;

while (toRun.Count != 0)
{
    var current = toRun.Dequeue();
    var (x, y, step, stage) = current;
    if (x < 0 || x > maxX || y < 0 || y > maxY || data[y][x] == '#')
        continue;

    if (stage == 1 && y == 0)
        stage = 2;

    if (y == maxY)
    {
        if (stage == 0)
        {
            if (!foundPart1)
            {
                Console.WriteLine($"part 1: {step - 1}");
                foundPart1 = true;
            }
            stage = 1;
        }
        else if (stage == 2)
        {
            Console.WriteLine($"part 2: {step - 1}");
            break;
        }
    }

    if (visited.Contains(current))
        continue;

    _ = visited.Add(current);

    var obstacles = occupiedSpotsByRound[step % occupiedSpotsByRound.Count];
    if (!obstacles.Contains((x, y)))
        toRun.Enqueue((x, y, step + 1, stage));
    if (!obstacles.Contains((x + 1, y)))
        toRun.Enqueue((x + 1, y, step + 1, stage));
    if (!obstacles.Contains((x - 1, y)))
        toRun.Enqueue((x - 1, y, step + 1, stage));
    if (!obstacles.Contains((x, y + 1)))
        toRun.Enqueue((x, y + 1, step + 1, stage));
    if (!obstacles.Contains((x, y - 1)))
        toRun.Enqueue((x, y - 1, step + 1, stage));
}
