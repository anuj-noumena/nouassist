namespace NouAssist.Contacts;

public static class ContactsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Contacts";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Contacts";
}
