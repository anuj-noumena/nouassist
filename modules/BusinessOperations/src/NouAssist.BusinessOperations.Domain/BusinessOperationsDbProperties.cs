namespace NouAssist.BusinessOperations;

public static class BusinessOperationsDbProperties
{
    public static string DbTablePrefix { get; set; } = "BusinessOperations";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "BusinessOperations";
}
