namespace NouAssist.Notification.Reminders;

public static class RemindersDbProperties
{
    public static string DbTablePrefix { get; set; } = "Reminders";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Reminders";
}
