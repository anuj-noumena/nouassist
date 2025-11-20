namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class GetDiscussionThreadListDto : PagedAndSortedResultRequestDto
    {
        public Guid DiscussionBoardId { get; set; }
        public bool? OnlyOpen { get; set; }
    }
}
