namespace NouAssist.IdentityAccess;

public static class IdentityAccessDbProperties
{
    public static string DbTablePrefix { get; set; } = "IdentityAccess";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "IdentityAccess";
}
