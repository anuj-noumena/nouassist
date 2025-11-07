namespace NouAssist.CollaborationHub.TeamCollaboration;

public static class TeamCollaborationDbProperties
{
    public static string DbTablePrefix { get; set; } = "TeamCollaboration";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "TeamCollaboration";
}
