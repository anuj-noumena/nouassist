using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class DiscussionThreadDto : EntityDto<Guid>
    {
        public Guid DiscussionBoardId { get; set; }
        public string Subject { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorUserName { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ClosedTime { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
