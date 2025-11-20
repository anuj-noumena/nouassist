namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class CommentDto : EntityDto<Guid>
    {
        public Guid ThreadId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorUserName { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? LastEditTime { get; set; }
        public List<Guid> MentionedUserIds { get; set; }
        public List<string> MentionedUserNames { get; set; }
    }
}
