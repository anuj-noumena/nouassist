using System;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class DiscussionBoardDto : FullAuditedEntityDto<Guid>
    {
        public TargetResourceType ResourceType { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceDisplayName { get; set; }
        public bool IsActive { get; set; }
        public int ThreadCount { get; set; }
        public int CommentCount { get; set; }
    }
}
