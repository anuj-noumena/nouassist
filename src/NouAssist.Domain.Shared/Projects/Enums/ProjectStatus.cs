namespace NouAssist.Projects.Enums
{
    public enum ProjectStatus : byte
    {
        Template = 0,
        Planned = 1,
        Active = 2,
        OnHold = 3,
        Completed = 4,
        Cancelled = 5
    }

    public enum ProjectHealth : byte
    {
        Healthy = 1,
        AtRisk = 2,
        Critical = 3,
        OnHold = 4,
        Completed = 5,
        Cancelled = 6
    }

    public enum EisenhowerQuadrant : byte
    {
        Q1DoFirst = 1,      // Important & Urgent
        Q2Schedule = 2,     // Important & Not Urgent
        Q3Delegate = 3,     // Not Important & Urgent
        Q4Eliminate = 4     // Not Important & Not Urgent
    }
}
