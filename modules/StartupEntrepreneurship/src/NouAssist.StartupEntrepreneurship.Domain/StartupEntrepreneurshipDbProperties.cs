namespace NouAssist.StartupEntrepreneurship;

public static class StartupEntrepreneurshipDbProperties
{
    public static string DbTablePrefix { get; set; } = "StartupEntrepreneurship";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "StartupEntrepreneurship";
}
