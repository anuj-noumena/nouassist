namespace NouAssist.CollaborationHub.TeamCollaboration;

public static class MeetingsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Meetings";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Meetings";
}
