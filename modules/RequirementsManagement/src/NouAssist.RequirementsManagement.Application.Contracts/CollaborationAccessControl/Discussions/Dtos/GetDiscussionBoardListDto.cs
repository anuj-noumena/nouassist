using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class GetDiscussionBoardListDto : PagedAndSortedResultRequestDto
    {
        public TargetResourceType? ResourceType { get; set; }
        public bool? OnlyActive { get; set; }
    }
}
