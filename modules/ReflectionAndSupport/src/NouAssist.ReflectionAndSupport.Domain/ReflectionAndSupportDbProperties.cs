namespace NouAssist.ReflectionAndSupport;

public static class ReflectionAndSupportDbProperties
{
    public static string DbTablePrefix { get; set; } = "ReflectionAndSupport";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ReflectionAndSupport";
}
