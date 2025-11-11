namespace NouAssist.Customization;

public static class CustomizationDbProperties
{
    public static string DbTablePrefix { get; set; } = "Customization";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Customization";
}
