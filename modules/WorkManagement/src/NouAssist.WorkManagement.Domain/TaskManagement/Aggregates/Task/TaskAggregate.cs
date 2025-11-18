using System;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.Task
{
    /// <summary>
    /// The core aggregate representing a task blueprint (template).
    /// Manages identity, content, and primary organizational placement.
    /// </summary>
    public class TaskAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        // === IDENTITY ===
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Guid? TaskTypeId { get; private set; }

        // === CONTENT ===
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ExpectedResult { get; private set; }
        public string Notes { get; private set; }

        // === GTD ORGANIZATION ===
        public Guid? AreaId { get; private set; }
        public Guid? ContextId { get; private set; }

        // === STATE ===
        public bool IsActive { get; private set; }
        public int Version { get; private set; }
        
        // References to other aggregates
        public Guid? DefinitionId { get; private set; }
        public Guid? DefaultsId { get; private set; }
        public Guid? RecurrenceId { get; private set; }
        public Guid? HierarchyId { get; private set; }

        protected TaskAggregate() { }

        public TaskAggregate(
            Guid id,
            Guid ownerId,
            string title,
            Guid taskTypeId,
            Guid? tenantId = null,
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null) : base(id)
        {
            OwnerId = ownerId;
            TaskTypeId = taskTypeId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            TenantId = tenantId;
            ProjectId = projectId;
            AreaId = areaId;
            ContextId = contextId;
            
            IsActive = true;
            Version = 1;

            AddDistributedEvent(new TaskCreatedEto
            {
                TaskId = Id,
                Title = Title,
                OwnerId = OwnerId,
                TenantId = TenantId
            });
        }

        // === CONTENT & IDENTITY UPDATE METHODS ===
        public void UpdateContent(string? title, string? description, string? expectedResult, string? notes)
        {
            if (!string.IsNullOrWhiteSpace(title)) Title = title;
            if (description != null) Description = description;
            if (expectedResult != null) ExpectedResult = expectedResult;
            if (notes != null) Notes = notes;
            Version++;
        }
        
        public void UpdateTaskType(Guid? newTaskTypeId)
        {
            TaskTypeId = newTaskTypeId;
            Version++;
        }

        // === GTD ORGANIZATION METHODS ===
        public void AssignToArea(Guid areaId)
        {
            if (ProjectId.HasValue) throw new BusinessException("TaskManagement:CannotBeInBothProjectAndArea");
            AreaId = areaId;
            Version++;
        }
        
        public void MoveToProject(Guid projectId)
        {
            if (AreaId.HasValue) throw new BusinessException("TaskManagement:CannotBeInBothProjectAndArea");
            ProjectId = projectId;
            Version++;
        }

        public void AssignToContext(Guid contextId)
        {
            ContextId = contextId;
            Version++;
        }

        // === STATE MANAGEMENT ===
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}