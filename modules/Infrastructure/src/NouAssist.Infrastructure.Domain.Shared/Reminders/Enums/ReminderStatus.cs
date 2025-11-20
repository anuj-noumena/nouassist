namespace NouAssist.Notification.Reminders.Enums
{
    public enum ReminderStatus : byte
    {
        Pending = 1,
        Due = 2,
        Overdue = 3,
        Sent = 4,
        Snoozed = 5,
        Cancelled = 6
    }
}
