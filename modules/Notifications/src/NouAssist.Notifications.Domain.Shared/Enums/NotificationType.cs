namespace NouAssist.Notifications.Enums
{
    public enum NotificationType : byte
    {
        TaskAssigned = 1,
        TaskDueSoon = 2,
        TaskOverdue = 3,
        TaskCompleted = 4,
        TaskCommented = 5,
        TaskMentioned = 6,
        ProjectStatusChanged = 7,
        DelegationReceived = 8,
        DelegationAccepted = 9,
        DelegationDeclined = 10,
        CalendarEventReminder = 11,
        CalendarEventInvite = 12,
        SchedulingRequest = 13,
        CommentReply = 14,
        CommentMention = 15,
        HabitReminder = 16,
        RoutineReminder = 17,
        ReviewReminder = 18,
        EisenhowerQ1Overload = 19,
        AchievementUnlocked = 20,
        SystemAnnouncement = 21,
        Other = 99
    }
}
