namespace NouAssist.CoreProductivity.GoalTracking;

public static class GoalsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Goals";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Goals";
}
