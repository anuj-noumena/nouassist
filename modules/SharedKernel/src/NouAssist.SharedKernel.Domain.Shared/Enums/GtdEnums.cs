namespace NouAssist.Core.Enums
{
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
}
