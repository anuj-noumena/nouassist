using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.ProjectManagement.Events
{
    [EventName("NouAssist.CoreProductivity.ProjectManagement.Created")]
    public class ProjectCreatedEto : IMultiTenant
    {
        public Guid ProjectId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime OccurredAt { get; set; }
        public Guid? AreaId { get; set; }
    }

    [EventName("NouAssist.Projects.ProjectCompleted")]
    public class ProjectCompletedEto : IMultiTenant
    {
        public Guid ProjectId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CompletedAt { get; set; }
        public double Progress { get; set; }
        public double SuccessCriteriaCompletionRate { get; set; }

        public ProjectCompletedEto()
        {
        }

        public ProjectCompletedEto(
            Guid projectId,
            DateTime completedAt,
            double progress,
            double successCriteriaCompletionRate)
        {
            ProjectId = projectId;
            CompletedAt = completedAt;
            Progress = progress;
            SuccessCriteriaCompletionRate = successCriteriaCompletionRate;
        }
    }
    
    [EventName("NouAssist.CoreProductivity.ProjectManagement.StatusChanged")]
    public class ProjectStatusChangedEto : IMultiTenant
    {
        public Guid ProjectId { get; set; }
        public Guid? TenantId { get; set; }
        public ProjectStatus OldStatus { get; set; }
        public ProjectStatus NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; }
    }

    [EventName("NouAssist.CoreProductivity.ProjectManagement.Sprints.Completed")]
    public class SprintCompletedEto : IMultiTenant
    {
        public Guid SprintId { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid? TenantId { get; private set; }
        public DateTime CompletedAt { get; private set; }
        public int PlannedStoryPoints { get; private set; }
        public int CompletedStoryPoints { get; private set; }
        public double CompletionRate { get; private set; }

        // Constructor matching the usage in ProjectSprint.Complete()
        public SprintCompletedEto(
            Guid sprintId,
            Guid projectId,
            Guid? tenantId,
            DateTime completedAt,
            int plannedStoryPoints,
            int completedStoryPoints,
            double completionRate)
        {
            SprintId = sprintId;
            ProjectId = projectId;
            TenantId = tenantId;
            CompletedAt = completedAt;
            PlannedStoryPoints = plannedStoryPoints;
            CompletedStoryPoints = completedStoryPoints;
            CompletionRate = completionRate;
        }

        // Parameterless constructor for serialization
        protected SprintCompletedEto() { }
    }
}
