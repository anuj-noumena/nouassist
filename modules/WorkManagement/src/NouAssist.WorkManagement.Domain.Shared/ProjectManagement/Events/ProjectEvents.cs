using System;
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
    }

    [EventName("NouAssist.CoreProductivity.ProjectManagement.Completed")]
    public class ProjectCompletedEto : IMultiTenant
    {
        public Guid ProjectId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CompletedAt { get; set; }
        public double FinalProgress { get; set; }
        public string CompletedBy { get; set; }
    }

    [EventName("NouAssist.CoreProductivity.ProjectManagement.StatusChanged")]
    public class ProjectStatusChangedEto : IMultiTenant
    {
        public Guid ProjectId { get; set; }
        public Guid? TenantId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; }
    }

    [EventName("NouAssist.CoreProductivity.ProjectManagement.Sprints.Completed")]
    public class SprintCompletedEto : IMultiTenant
    {
        public Guid SprintId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CompletedAt { get; set; }
        public int PlannedStoryPoints { get; set; }
        public int CompletedStoryPoints { get; set; }
        public double CompletionRate { get; set; }
    }
}
