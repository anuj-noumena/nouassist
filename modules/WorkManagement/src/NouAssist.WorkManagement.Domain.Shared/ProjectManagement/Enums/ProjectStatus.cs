namespace NouAssist.WorkManagement.ProjectManagement.Enums
{
    public enum ProjectStatus : byte
    {
        Template = 0,
        Planned = 1,
        Initiation = 2,
        Active = 3,//executing
        OnHold = 4,
        Monitoring = 5,
        Closing = 6,
        Completed = 7,
        Cancelled = 8
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
