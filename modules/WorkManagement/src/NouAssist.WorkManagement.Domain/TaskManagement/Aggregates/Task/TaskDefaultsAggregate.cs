using System;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.Task
{
    /// <summary>
    /// Manages the default values for instances created from a task blueprint.
    /// </summary>
    public class TaskDefaultsAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskId { get; private set; }
        
        public double DefaultImportance { get; private set; }
        public double DefaultUrgency { get; private set; }
        public DateTime? DefaultDueDate { get; private set; }
        public int? DefaultEstimateMinutes { get; private set; }
        public Guid? DefaultWorkflowId { get; private set; }
        public Guid? DefaultAssigneeId { get; private set; }
        public Guid? DefaultAreaId { get; private set; }
        public Guid? DefaultContextId { get; private set; }
        public ComplexityEstimation DefaultComplexityEstimation { get; private set; }
        public CognitiveLoad? DefaultCognitiveLoad { get; private set; }

        protected TaskDefaultsAggregate() { }

        public TaskDefaultsAggregate(Guid id, Guid TaskId) : base(id)
        {
            TaskId = TaskId;
            DefaultImportance = 0.5;
            DefaultUrgency = 0.5;
            DefaultComplexityEstimation = ComplexityEstimation.Unknown();
        }

        // === DEFAULTS UPDATE METHODS ===
        public void UpdateDefaultPriority(double importance, double urgency)
        {
            DefaultImportance = ImportanceUrgencyMatrix.ValidateImportance(importance);
            DefaultUrgency = ImportanceUrgencyMatrix.ValidateUrgency(urgency);
            AddDistributedEvent(new TaskDefaultsUpdatedEto { TaskDefaultsId = Id, Type = "Priority" });
        }

        public void UpdateDefaultSchedule(DateTime? defaultDueDate, int? defaultEstimateMinutes)
        {
            DefaultDueDate = defaultDueDate;
            DefaultEstimateMinutes = defaultEstimateMinutes;
            AddDistributedEvent(new TaskDefaultsUpdatedEto { TaskDefaultsId = Id, Type = "Schedule" });
        }

        public void UpdateDefaultAssignee(Guid? defaultAssigneeId)
        {
            DefaultAssigneeId = defaultAssigneeId;
            AddDistributedEvent(new TaskDefaultsUpdatedEto { TaskDefaultsId = Id, Type = "Assignee" });
        }

        public void UpdateDefaultWorkflow(Guid? defaultWorkflowId)
        {
            DefaultWorkflowId = defaultWorkflowId;
            AddDistributedEvent(new TaskDefaultsUpdatedEto { TaskDefaultsId = Id, Type = "Workflow" });
        }
        
        public void SetDefaultOrganization(Guid? defaultAreaId, Guid? defaultContextId)
        {
            DefaultAreaId = defaultAreaId;
            DefaultContextId = defaultContextId;
            AddDistributedEvent(new TaskDefaultsUpdatedEto { TaskDefaultsId = Id, Type = "Organization" });
        }
    }
}