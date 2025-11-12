namespace NouAssist.WorkManagement.GTD.Enums
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
    
    public enum GtdHorizonLevel : byte
    {
        Horizon0Ground = 0,         // Current actions
        Horizon1Projects = 1,       // Multi-step outcomes
        Horizon2Areas = 2,          // Areas of focus
        Horizon3Goals = 3,          // 1-2 year goals
        Horizon4Vision = 4,         // Long-term vision
        Horizon5Purpose = 5         // Life purpose
    }

    public enum HorizonReviewFrequency : byte
    {
        Daily = 1,
        Weekly = 2,
        Biweekly = 3,
        Monthly = 4,
        Quarterly = 5,
        Semiannually = 6,
        Annually = 7,
        Custom = 8
    }

    public enum SomedayMaybeStatus : byte
    {
        Active = 1,
        Promoted = 2,
        Archived = 3,
        OnHold = 4
    }

    public enum TicklerStatus : byte
    {
        Scheduled = 1,
        Surfaced = 2,
        Actioned = 3,
        Dismissed = 4,
        Snoozed = 5
    }

    public enum TicklerActionType : byte
    {
        Review = 1,
        Decide = 2,
        StartTask = 3,
        CheckProgress = 4,
        SendReminder = 5,
        FollowUp = 6,
        Custom = 99
    }

    public enum WaitingForType : byte
    {
        User = 1,
        External = 2,
        Resource = 3
    }

    public enum WaitingForStatus : byte
    {
        Active = 1,
        Resolved = 2,
        OnHold = 3,
        Cancelled = 4,
        Escalated = 5
    }
    
    public enum FollowupStatus : byte
    {
        Scheduled = 1,
        Dismissed = 2,
        Done = 3
    }
}
