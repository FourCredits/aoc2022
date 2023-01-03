using AOC.Common;

using Day20;

var input = File.ReadAllLines("inputs/20.txt").Select(int.Parse).Enumerate();
var numbers = input.ToList();
WithoutKey.Mix(numbers, input);
var part1 = WithoutKey.FindCoordinateSum(numbers);
Console.WriteLine($"part 1: {part1}"); // 9687
var largeInput = input.Select(pair => (pair.index, pair.item * 811589153L));
var largeNumbers = largeInput.ToList();
foreach (var _ in Enumerable.Range(0, 10))
    WithKey.Mix(largeNumbers, largeInput);
var part2 = WithKey.FindCoordinateSum(largeNumbers);
Console.WriteLine($"part 2: {part2}"); // 1338310513297
