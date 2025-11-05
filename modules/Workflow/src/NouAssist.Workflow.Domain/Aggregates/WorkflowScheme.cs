using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Workflow.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Workflow.Aggregates
{
    public class WorkflowScheme : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public Guid ProjectId { get; private set; }
        public Guid? DefaultWorkflowId { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDraft { get; private set; }

        private readonly List<WorkflowSchemeMapping> _mappings = new();
        public IReadOnlyCollection<WorkflowSchemeMapping> Mappings => 
            _mappings.AsReadOnly();

        protected WorkflowScheme() { }

        public WorkflowScheme(
            Guid id,
            string name,
            Guid projectId,
            string description = null,
            Guid? defaultWorkflowId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ProjectId = projectId;
            Description = description;
            DefaultWorkflowId = defaultWorkflowId;
            IsDraft = true;
            IsActive = false;
        }

        public void AddMapping(WorkflowSchemeMapping mapping)
        {
            Check.NotNull(mapping, nameof(mapping));

            if (_mappings.Any(m => m.TaskTypeId == mapping.TaskTypeId))
            {
                throw new BusinessException("Workflow:DuplicateTaskTypeMapping");
            }

            _mappings.Add(mapping);
        }

        public void RemoveMapping(Guid taskTypeId)
        {
            var mapping = _mappings.FirstOrDefault(m => m.TaskTypeId == taskTypeId);
            if (mapping != null)
            {
                _mappings.Remove(mapping);
            }
        }

        public Guid GetWorkflowForTaskType(Guid taskTypeId)
        {
            var mapping = _mappings.FirstOrDefault(m => 
                m.TaskTypeId == taskTypeId && m.IsActive);

            if (mapping != null)
            {
                return mapping.WorkflowId;
            }

            if (DefaultWorkflowId.HasValue)
            {
                return DefaultWorkflowId.Value;
            }

            throw new BusinessException("Workflow:NoWorkflowForTaskType");
        }

        public void SetDefaultWorkflow(Guid workflowId)
        {
            DefaultWorkflowId = workflowId;
        }

        public void Publish()
        {
            if (!DefaultWorkflowId.HasValue && !_mappings.Any())
            {
                throw new BusinessException("Workflow:CannotPublishEmptyScheme");
            }

            IsDraft = false;
        }

        public void Activate()
        {
            if (IsDraft)
            {
                throw new BusinessException("Workflow:CannotActivateDraft");
            }

            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
