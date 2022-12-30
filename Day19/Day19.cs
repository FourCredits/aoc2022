﻿using Day19;

var practice = args.Length > 0 && args[0] == "practice";
var blueprints = File
    .ReadAllLines(practice ? "inputs/practice.txt" : "inputs/19.txt")
    .Select(Blueprint.Parse);
var part1 = blueprints
    .Select(blueprint => SearchOptionSpace(blueprint).Max() * blueprint.Id)
    .Sum();
Console.WriteLine($"part 1: {part1}");

static IEnumerable<int> SearchOptionSpace(Blueprint blueprint)
{
    Console.WriteLine(blueprint);
    Stack<State> states = new();
    states.Push(State.Start);
    HashSet<State> visited = new() { State.Start };
    while (states.TryPop(out var state))
    {
        // Console.WriteLine(state);
        if (state.Time == 24)
        {
            yield return state.Geodes;
            continue;
        }
        foreach (var possibleState in state.NextStates(blueprint))
            if (possibleState is State s && visited.Add(s))
                states.Push(s);
    }
}

/*
Okay. So, the trick is that you have to build each blueprint optimally. And that
doesn't necessarily mean "build the highest tier robot you can at this moment".

My working theory for the best strategy is:

- Try to build the highest tier robot you can based on the robots you have
- Building obsidian and geode robots requires collecting enough clay and
  obsidian, respectively, but also getting enough clay. But you could collect
  enough ore that, in the time you waited for enough of the higher tier
  material, you could also have built a clay or ore robot in that time, and
  still have enough ore to make the high-tier robot.

There is, of course, the glaring problem that you *could* try and figure out the
optimal strategy. But it's probably easier to just search the entire option
space, and find the one that produces the best result. I might try that first.
*/
