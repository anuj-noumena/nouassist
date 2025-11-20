namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Events
{
    /// <summary>
    /// Event raised when a new comment is added to a discussion thread.
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.CollaborationAccessControl.CommentAdded")]
    public class CommentAddedEto : NotificationEtoBase
    {
        public Guid DiscussionBoardId { get; set; }
        public Guid ThreadId { get; set; }
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public List<Guid> MentionedUserIds { get; set; } = new();
    }
}
