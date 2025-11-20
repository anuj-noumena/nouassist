namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class CreateDiscussionThreadDto
    {
        [Required]
        public Guid DiscussionBoardId { get; set; }

        [Required]
        [StringLength(CollaborationConsts.MaxThreadSubjectLength)]
        public string Subject { get; set; }

        [Required]
        [StringLength(CollaborationConsts.MaxCommentContentLength)]
        public string InitialComment { get; set; }
    }
}
