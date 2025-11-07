namespace NouAssist.CollaborationHub.ContactManagement;

public static class ContactManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "ContactManagement";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ContactManagement";
}
