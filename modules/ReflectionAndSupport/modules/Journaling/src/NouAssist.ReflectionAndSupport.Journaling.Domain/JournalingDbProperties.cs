namespace NouAssist.ReflectionAndSupport.Journaling;

public static class JournalingDbProperties
{
    public static string DbTablePrefix { get; set; } = "Journaling";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Journaling";
}
