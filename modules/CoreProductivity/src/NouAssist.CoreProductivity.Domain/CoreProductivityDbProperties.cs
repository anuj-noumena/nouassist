namespace NouAssist.CoreProductivity;

public static class CoreProductivityDbProperties
{
    public static string DbTablePrefix { get; set; } = "CoreProductivity";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "CoreProductivity";
}
