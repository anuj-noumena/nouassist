using System;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance
{
    /// <summary>
    /// Manages the organizational placement of a task within GTD constructs (Project, Area, Context).
    /// </summary>
    public class TaskOrganizationAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskInstanceId { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Guid? AreaId { get; private set; }
        public Guid? ContextId { get; private set; }

        protected TaskOrganizationAggregate() { }

        public TaskOrganizationAggregate(Guid id, Guid taskInstanceId) : base(id)
        {
            TaskInstanceId = taskInstanceId;
        }

        // === Organization Methods ===
        public void MoveToProject(Guid projectId)
        {
            if (AreaId.HasValue) throw new BusinessException("TaskManagement:CannotBeInBothProjectAndArea");
            ProjectId = projectId;
            AreaId = null; // Ensure exclusivity
            AddDistributedEvent(new TaskMovedToProjectEto { TaskOrganizationAggregateId = Id, ProjectId = projectId });
        }

        public void MoveToArea(Guid areaId)
        {
            if (ProjectId.HasValue) throw new BusinessException("TaskManagement:CannotBeInBothProjectAndArea");
            AreaId = areaId;
            ProjectId = null; // Ensure exclusivity
            AddDistributedEvent(new TaskMovedToAreaEto { TaskOrganizationAggregateId = Id, AreaId = areaId });
        }

        public void AssignToContext(Guid contextId)
        {
            ContextId = contextId;
            AddDistributedEvent(new TaskContextChangedEto { TaskOrganizationAggregateId = Id, ContextId = contextId });
        }

        public void RemoveFromProject()
        {
            ProjectId = null;
            AddDistributedEvent(new TaskRemovedFromProjectEto { TaskOrganizationAggregateId = Id });
        }

        public void RemoveFromArea()
        {
            AreaId = null;
            AddDistributedEvent(new TaskRemovedFromAreaEto { TaskOrganizationAggregateId = Id });
        }

        public void RemoveFromContext()
        {
            ContextId = null;
            AddDistributedEvent(new TaskRemovedFromContextEto { TaskOrganizationAggregateId = Id });
        }
    }
}