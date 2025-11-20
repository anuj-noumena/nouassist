namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Enums
{
    /// <summary>
    /// Defines the types of events that can trigger notifications.
    /// </summary>
    public enum NotificationEventType
    {
        ResourceCreated = 1,
        ResourceUpdated = 2,
        ResourceDeleted = 3,
        CommentAdded = 4,
        MentionedInComment = 5,
        ReviewAssigned = 6,
        ReviewCompleted = 7,
        ApprovalRequired = 8,
        ApprovalGranted = 9,
        ApprovalRejected = 10,
        PermissionChanged = 11
    }
}
