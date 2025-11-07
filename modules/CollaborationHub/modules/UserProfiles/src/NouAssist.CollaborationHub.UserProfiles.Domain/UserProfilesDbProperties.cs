namespace NouAssist.CollaborationHub.UserProfiles;

public static class UserProfilesDbProperties
{
    public static string DbTablePrefix { get; set; } = "UserProfiles";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "UserProfiles";
}
