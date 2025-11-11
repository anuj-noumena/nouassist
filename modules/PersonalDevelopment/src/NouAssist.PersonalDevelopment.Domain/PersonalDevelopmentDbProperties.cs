namespace NouAssist.PersonalDevelopment;

public static class PersonalDevelopmentDbProperties
{
    public static string DbTablePrefix { get; set; } = "PersonalDevelopment";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "PersonalDevelopment";
}
