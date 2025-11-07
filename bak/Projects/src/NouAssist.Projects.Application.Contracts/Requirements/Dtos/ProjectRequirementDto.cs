using System;
using NouAssist.Projects.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Projects.Requirements.Dtos
{
    public class ProjectRequirementDto : FullAuditedEntityDto<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectRequirementType RequirementType { get; set; }
        public ProjectRequirementStatus Status { get; set; }
    }
}
