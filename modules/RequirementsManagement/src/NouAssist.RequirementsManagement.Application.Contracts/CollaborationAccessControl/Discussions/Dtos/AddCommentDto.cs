namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class AddCommentDto
    {
        [Required]
        public Guid ThreadId { get; set; }

        [Required]
        [StringLength(CollaborationConsts.MaxCommentContentLength)]
        public string Content { get; set; }
    }
}
