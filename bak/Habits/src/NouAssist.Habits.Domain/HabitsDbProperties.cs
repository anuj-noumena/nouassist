namespace NouAssist.Habits;

public static class HabitsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Habits";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Habits";
}
