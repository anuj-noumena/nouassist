using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class GetDiscussionBoardListDto : PagedAndSortedResultRequestDto
    {
        public TargetResourceType? ResourceType { get; set; }
        public bool? OnlyActive { get; set; }
    }
}
