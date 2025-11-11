namespace NouAssist.WorkManagement.TaskManagement.Enums
{
    /// <summary>
    /// Types of changes that can be recorded in task history
    /// </summary>
    public enum TaskHistoryChangeType : byte
    {
        Created = 1,
        StatusChanged = 2,
        Assigned = 3,
        Unassigned = 4,
        Reassigned = 5,
        Delegated = 6,
        DelegationAccepted = 7,
        DelegationDeclined = 8,
        DelegationRevoked = 9,
        Started = 10,
        Completed = 11,
        Cancelled = 12,
        Reopened = 13,
        Blocked = 14,
        Unblocked = 15,
        PriorityChanged = 16,
        DueDateChanged = 17,
        ProgressUpdated = 18,
        StepCompleted = 19,
        StepUncompleted = 20,
        CriterionMet = 21,
        CriterionUnmet = 22,
        Flagged = 23,
        Unflagged = 24,
        MarkedAsNextAction = 25,
        UnmarkedAsNextAction = 26,
        CommentAdded = 27,
        AttachmentAdded = 28,
        WatcherAdded = 29,
        WatcherRemoved = 30,
        DependencyAdded = 31,
        DependencyRemoved = 32,
        MovedToProject = 33,
        TimeLogged = 34,
        DiscussionLocked = 35,
        DiscussionUnlocked = 36,
        FieldUpdated = 37,
        WorkflowTransition = 38,
        Other = 99
    }
}
