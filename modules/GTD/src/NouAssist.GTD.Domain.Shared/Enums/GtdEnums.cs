namespace NouAssist.GTD.Enums
{
    public enum TicklerRelatedEntityType : byte
    {
        Task = 1,
        Project = 2,
        Pbi = 3,
        Sprint = 4,
        CalendarEvent = 5,
        Someday = 6,
        None = 7
    }

    public enum ResultingEntityType : byte
    {
        None = 0,
        Task = 1,
        Project = 2,
        CalendarEvent = 3,
        SomedayMaybe = 4
    }

    public enum TicklerCategory : byte
    {
        Gtd = 1,
        Task = 2,
        Project = 3,
        Communication = 4,
        Calendar = 5,
        Agile = 6,
        Sprint = 7,
        Review = 8,
        Planning = 9,
        SomedayMaybe = 10,
        Admin = 11,
        Habit = 12,
        WaitingFor = 13,
        Other = 14
    }

    public enum TicklerProcessingAction : byte
    {
        Dismissed = 1,
        Completed = 2,
        Archived = 3,
        Snoozed = 4,
        ConvertedToTask = 5,
        ConvertedToProject = 6,
        ConvertedToCalendarEvent = 7,
        ConvertedToSomedayMaybe = 8,
        ConvertedToResource = 9,
        LinkedToTask = 10,
        LinkedToProject = 11,
        LinkedToEvent = 12,
        CompletedImmediately = 13,
        Delegated = 14,
        NeedsMoreInfo = 15,
        Escalated = 16,
        AutoProcessed = 17,
        Expired = 18,
        FollowUpScheduled = 19
    }

    public enum GtdLowerEntityType : byte
    {
        GtdHorizon = 1,
        Goal = 2,
        Area = 3,
        Project = 4,
        Task = 5,
        Habit = 6,
        Pbi = 7,
        Sprint = 8,
        SomedayMaybe = 9,
        Tickler = 10
    }
}
