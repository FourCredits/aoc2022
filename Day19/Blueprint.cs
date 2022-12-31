namespace Day19;

public readonly record struct Blueprint(
    int Id,
    int OreForOreRobot,
    int OreForClayRobot,
    int OreForObsidianRobot,
    int ClayForObsidianRobot,
    int OreForGeodeRobot,
    int ObsidianForGeodeRobot)
{
    public int MaxOreCost => new[] {
        OreForOreRobot,
        OreForClayRobot,
        OreForObsidianRobot,
        OreForGeodeRobot}.Max();

    public static Blueprint Parse(string s)
    {
        var nums = AOC.Common.Parse.ManyInts(s);
        return new Blueprint(
            nums[0],
            nums[1],
            nums[2],
            nums[3],
            nums[4],
            nums[5],
            nums[6]);
    }
}
