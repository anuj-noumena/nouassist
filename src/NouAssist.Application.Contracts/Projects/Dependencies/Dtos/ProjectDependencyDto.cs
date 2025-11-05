using System;
using NouAssist.Projects.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Projects.Dependencies.Dtos
{
    public class ProjectDependencyDto : FullAuditedEntityDto<Guid>
    {
        public Guid ProjectId { get; set; }
        public Guid DependsOnProjectId { get; set; }
        public ProjectDependencyType DependencyType { get; set; }
        public Guid? TenantId { get; set; }

        public int LagDays { get; set; }
        public bool IsBlocking { get; set; }
        public ProjectDependencyStatus Status { get; set; }

        public string Description { get; set; }
        public string Notes { get; set; }

        public DateTime? ExpectedResolutionDate { get; set; }
        public DateTime? ActualResolutionDate { get; set; }
        public string ResolvedBy { get; set; }

        public Guid? ApprovalRequestId { get; set; }

        // Computed
        public bool IsOverdue { get; set; }
        public string DependencyTypeDescription { get; set; }
    }
}
