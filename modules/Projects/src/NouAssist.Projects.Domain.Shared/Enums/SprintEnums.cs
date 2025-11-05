namespace NouAssist.Projects.Enums
{
    public enum SprintStatus : byte
    {
        Planned = 1,
        InProgress = 2,
        Completed = 3,
        Canceled = 4,
        OnHold = 5
    }

    public enum SprintHealth : byte
    {
        NotStarted = 1,
        OnTrack = 2,
        AtRisk = 3,
        BehindSchedule = 4,
        Completed = 5
    }
}
