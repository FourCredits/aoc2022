namespace Day19;

public readonly record struct State(
    int Geodes = 0,
    int Obsidian = 0,
    int Clay = 0,
    int Ore = 0,
    int OreRobots = 1,
    int GeodeRobots = 0,
    int ObsidianRobots = 0,
    int ClayRobots = 0,
    int TimeLeft = 0)
{
    public static State Start(int time) => new(OreRobots: 1, TimeLeft: time);

    public IEnumerable<State?> NextStates(Blueprint blueprint) => new[]
    {
        TickMakingGeodeRobot(
            blueprint.OreForGeodeRobot,
            blueprint.ObsidianForGeodeRobot),
        TickMakingObsidianRobot(
            blueprint.OreForObsidianRobot,
            blueprint.ClayForObsidianRobot),
        TickMakingClayRobot(blueprint.OreForClayRobot),
        TickMakingOreRobot(blueprint.OreForOreRobot),
        Tick(),
    };

    public State Tick() => this with
    {
        TimeLeft = TimeLeft - 1,
        Ore = Ore + OreRobots,
        Clay = Clay + ClayRobots,
        Obsidian = Obsidian + ObsidianRobots,
        Geodes = Geodes + GeodeRobots,
    };

    public State? TickMakingOreRobot(int oreNeeded) =>
        Ore < oreNeeded ? null : Tick().AddOreRobot(oreNeeded);

    public State? TickMakingClayRobot(int oreNeeded) =>
        Ore < oreNeeded ? null : Tick().AddClayRobot(oreNeeded);

    public State? TickMakingObsidianRobot(int oreNeeded, int clayNeeded) =>
        Ore < oreNeeded || Clay < clayNeeded
            ? null
            : Tick().AddObsidianRobot(oreNeeded, clayNeeded);

    public State? TickMakingGeodeRobot(int oreNeeded, int obsidianNeeded) =>
        Ore < oreNeeded || Obsidian < obsidianNeeded
            ? null
            : Tick().AddGeodeRobot(oreNeeded, obsidianNeeded);

    public State Minimise(Blueprint blueprint) => this with
    {
        OreRobots = Math.Min(OreRobots, blueprint.MaxOreCost),
        ClayRobots = Math.Min(ClayRobots, blueprint.ClayForObsidianRobot),
        ObsidianRobots = Math.Min(
            ObsidianRobots,
            blueprint.ObsidianForGeodeRobot),
        Ore = Math.Min(
            Ore,
            (TimeLeft * blueprint.MaxOreCost) - (OreRobots * (TimeLeft - 1))),
        Clay = Math.Min(
            Clay,
            (TimeLeft * blueprint.ClayForObsidianRobot) -
                (ClayRobots * (TimeLeft - 1))),
        Obsidian = Math.Min(
            Obsidian,
            (TimeLeft * blueprint.ObsidianForGeodeRobot) -
                (ObsidianRobots * (TimeLeft - 1))),
    };

    // adds a ore robot without checking the bounds
    private State AddOreRobot(int oreNeeded) => this with
    {
        Ore = Ore - oreNeeded,
        OreRobots = OreRobots + 1,
    };

    // adds a clay robot without checking the bounds
    private State AddClayRobot(int oreNeeded) => this with
    {
        Ore = Ore - oreNeeded,
        ClayRobots = ClayRobots + 1,
    };

    // adds a obsidian robot without checking the bounds
    private State AddObsidianRobot(int oreNeeded, int clayNeeded) =>
        this with
        {
            Ore = Ore - oreNeeded,
            Clay = Clay - clayNeeded,
            ObsidianRobots = ObsidianRobots + 1,
        };

    // adds a geode robot without checking the bounds
    private State AddGeodeRobot(int oreNeeded, int obsidianNeeded) => this with
    {
        Ore = Ore - oreNeeded,
        Obsidian = Obsidian - obsidianNeeded,
        GeodeRobots = GeodeRobots + 1,
    };
}
