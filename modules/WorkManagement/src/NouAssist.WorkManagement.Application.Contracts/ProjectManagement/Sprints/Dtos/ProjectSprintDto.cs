using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos
{
    public class ProjectSprintDto : FullAuditedEntityDto<Guid>
    {
        public Guid ProjectId { get; set; }
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string Goal { get; set; }
        public string ReleaseTheme { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int PlannedStoryPoints { get; set; }
        public int CompletedStoryPoints { get; set; }
        public int VelocityEstimate { get; set; }
        public int ActualVelocity { get; set; }

        public SprintStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string CompletedBy { get; set; }
        public string RetrospectiveNotes { get; set; }

        // Computed
        public int DurationInDays { get; set; }
        public bool IsActive { get; set; }
        public int? DaysRemaining { get; set; }
        public double CompletionRate { get; set; }
        public SprintHealth Health { get; set; }
    }
}
