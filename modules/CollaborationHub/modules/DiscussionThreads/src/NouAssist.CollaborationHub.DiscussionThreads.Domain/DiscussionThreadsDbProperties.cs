namespace NouAssist.CollaborationHub.DiscussionThreads;

public static class DiscussionThreadsDbProperties
{
    public static string DbTablePrefix { get; set; } = "DiscussionThreads";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "DiscussionThreads";
}
