namespace NouAssist.WorkManagement.ProjectManagement.Enums
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
}
