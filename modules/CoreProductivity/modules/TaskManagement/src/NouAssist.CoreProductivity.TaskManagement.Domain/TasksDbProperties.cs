namespace NouAssist.CoreProductivity.TaskManagement;

public static class TasksDbProperties
{
    public static string DbTablePrefix { get; set; } = "Tasks";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Tasks";
}
