namespace NouAssist.CoreProductivity.HabitSystems;

public static class HabitSystemsDbProperties
{
    public static string DbTablePrefix { get; set; } = "HabitSystems";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "HabitSystems";
}
