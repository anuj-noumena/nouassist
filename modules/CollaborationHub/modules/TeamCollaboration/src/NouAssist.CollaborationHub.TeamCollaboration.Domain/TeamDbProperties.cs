namespace NouAssist.CollaborationHub.TeamCollaboration;

public static class TeamDbProperties
{
    public static string DbTablePrefix { get; set; } = "Team";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Team";
}
