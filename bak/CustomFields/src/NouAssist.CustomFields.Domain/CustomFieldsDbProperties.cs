namespace NouAssist.CustomFields;

public static class CustomFieldsDbProperties
{
    public static string DbTablePrefix { get; set; } = "CustomFields";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "CustomFields";
}
