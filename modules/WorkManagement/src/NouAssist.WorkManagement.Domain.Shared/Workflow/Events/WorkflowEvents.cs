using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.Workflow.Events
{
    [EventName("NouAssist.Workflow.Transition.Executed")]
    public class WorkflowTransitionExecutedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid TransitionId { get; set; }
        public Guid FromStatusId { get; set; }
        public Guid ToStatusId { get; set; }
        public Guid ExecutedBy { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ExecutedAt { get; set; }
        public string Comment { get; set; }
    }

    [EventName("NouAssist.Workflow.Status.Changed")]
    public class WorkflowStatusChangedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid OldStatusId { get; set; }
        public Guid NewStatusId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
