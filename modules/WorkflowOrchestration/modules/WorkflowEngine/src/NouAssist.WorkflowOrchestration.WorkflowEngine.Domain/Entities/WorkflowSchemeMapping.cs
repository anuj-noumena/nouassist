using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Entities
{
    public class WorkflowSchemeMapping : Entity<Guid>
    {
        public Guid WorkflowSchemeId { get; private set; }
        public Guid TaskTypeId { get; private set; }
        public Guid WorkflowId { get; private set; }
        public bool IsActive { get; set; }

        protected WorkflowSchemeMapping() { }

        public WorkflowSchemeMapping(
            Guid id,
            Guid workflowSchemeId,
            Guid taskTypeId,
            Guid workflowId) : base(id)
        {
            WorkflowSchemeId = workflowSchemeId;
            TaskTypeId = taskTypeId;
            WorkflowId = workflowId;
            IsActive = true;
        }
    }
}
