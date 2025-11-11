namespace NouAssist.WorkManagement.ProjectManagement.Enums
{
    public enum ProjectDependencyType : byte
    {
        FinishToStart = 1,
        StartToStart = 2,
        FinishToFinish = 3,
        StartToFinish = 4,
        Deliverable = 5,
        Resource = 6,
        Approval = 7,
        Information = 8
    }

    public enum ProjectDependencyStatus : byte
    {
        Active = 1,
        Resolved = 2,
        Obsolete = 3,
        Blocked = 4
    }
}
