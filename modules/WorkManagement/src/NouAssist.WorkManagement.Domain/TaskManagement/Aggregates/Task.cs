using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Events;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// Task Blueprint - Template with GTD Area support
    /// </summary>
    public class Task : FullAuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        // === IDENTITY ===
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskTypeId { get; set; }

        // === CONTENT ===
        public string Title { get; private set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }

        // === GTD ORGANIZATION ===
        public Guid? AreaId { get; set; } // GTD Area of Responsibility
        public Guid? ContextId { get; set; } // GTD Context (where/with what)
        
        // === DEFAULTS FOR INSTANCES ===
        public double DefaultImportance { get; private set; }
        public double DefaultUrgency { get; private set; }
        public DateTime? DefaultDueDate {get; private set;}
        public int? DefaultEstimateMinutes { get; set; }
        public Guid? DefaultWorkflowId { get; set; }
        public Guid? DefaultAssigneeId { get; set; }
        public Guid? DefaultAreaId { get; set; } // Default area for instances
        public Guid? DefaultContextId { get; set; } // Default context for instances

        // === RECURRENCE ===
        public RecurrenceRule RecurrenceRule { get; private set; }

        // === STATE ===
        public bool IsActive { get; private set; }
        public int Version { get; private set; }

        // NOTE: Checklist and Acceptance Criteria have been moved to TaskTemplate aggregate
        // This aggregate now focuses on core blueprint properties only

        protected Task() { }

        public Task(
            Guid id,
            Guid ownerId,
            string title,
            Guid? tenantId = null,
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            double defaultImportance = 0.5,
            double defaultUrgency = 0.5) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            TenantId = tenantId;
            ProjectId = projectId;
            AreaId = areaId;
            ContextId = contextId;
            
            SetDefaults(defaultImportance, defaultUrgency);
            
            IsActive = true;
            Version = 1;

            AddDistributedEvent(new TaskCreatedEto
            {
                TaskId = Id,
                Title = Title,
                OwnerId = OwnerId,
                AreaId = AreaId,
                ProjectId = ProjectId,
                ContextId = ContextId,
                TenantId = TenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        public void SetDefaults(double importance, double urgency)
        {
            DefaultImportance = ImportanceUrgencyMatrix.ValidateImportance(importance);
            DefaultUrgency = ImportanceUrgencyMatrix.ValidateUrgency(urgency);
        }

        // === GTD ORGANIZATION METHODS ===
        
        /// <summary>
        /// Assigns this task to a GTD Area of Responsibility
        /// </summary>
        public void AssignToArea(Guid areaId)
        {
            var oldAreaId = AreaId;
            AreaId = areaId;
            Version++;
            
            AddDistributedEvent(new TaskAreaChangedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldAreaId = oldAreaId,
                NewAreaId = areaId,
                Version = Version,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes area assignment (makes task area-less)
        /// </summary>
        public void RemoveFromArea()
        {
            if (!AreaId.HasValue)
            {
                return;
            }

            var oldAreaId = AreaId;
            AreaId = null;
            Version++;
            
            AddDistributedEvent(new TaskAreaChangedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldAreaId = oldAreaId,
                NewAreaId = null,
                Version = Version,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Assigns this task to a GTD Context
        /// </summary>
        public void AssignToContext(Guid contextId)
        {
            var oldContextId = ContextId;
            ContextId = contextId;
            Version++;
            
            AddDistributedEvent(new TaskContextChangedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldContextId = oldContextId,
                NewContextId = contextId,
                Version = Version,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes context assignment
        /// </summary>
        public void RemoveFromContext()
        {
            if (!ContextId.HasValue)
            {
                return;
            }

            var oldContextId = ContextId;
            ContextId = null;
            Version++;
            
            AddDistributedEvent(new TaskContextChangedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldContextId = oldContextId,
                NewContextId = null,
                Version = Version,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Sets default GTD organization for instances created from this blueprint
        /// </summary>
        public void SetDefaultOrganization(Guid? defaultAreaId, Guid? defaultContextId)
        {
            DefaultAreaId = defaultAreaId;
            DefaultContextId = defaultContextId;
            Version++;
        }

        /// <summary>
        /// Moves task from project to area (or vice versa)
        /// In GTD: Projects are specific outcomes, Areas are ongoing responsibilities
        /// </summary>
        public void MoveToArea(Guid areaId)
        {
            if (ProjectId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea",
                    "Task cannot belong to both a Project and an Area. Remove from project first.");
            }
            
            AssignToArea(areaId);
        }

        /// <summary>
        /// Moves task from area to project
        /// </summary>
        public void MoveToProject(Guid projectId, Guid workflowStatusId)
        {
            if (AreaId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea",
                    "Task cannot belong to both a Project and an Area. Remove from area first.");
            }
            
            var previousAreaId = AreaId;
            ProjectId = projectId;
            Version++;
            
            AddDistributedEvent(new TaskMovedToProjectEto
            {
                TaskId = Id,
                TenantId = TenantId,
                ProjectId = projectId,
                PreviousAreaId = previousAreaId,
                Version = Version,
                MovedAt = DateTime.UtcNow
            });
        }

        // NOTE: Checklist and Acceptance Criteria management methods have been moved to TaskTemplate aggregate

        // === RECURRENCE ===
        public void SetRecurrence(RecurrenceRule rule)
        {
            RecurrenceRule = rule;
            Version++;
            
            AddDistributedEvent(new TaskRecurrenceSetEto
            {
                TaskId = Id,
                TenantId = TenantId,
                RecurrencePattern = rule?.ToString(),
                Version = Version,
                SetAt = DateTime.UtcNow
            });
        }

        public void ClearRecurrence()
        {
            RecurrenceRule = null;
            Version++;
            
            AddDistributedEvent(new TaskRecurrenceClearedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                Version = Version,
                ClearedAt = DateTime.UtcNow
            });
        }

        // === STATE MANAGEMENT ===
        public void Activate()
        {
            IsActive = true;
            
            AddDistributedEvent(new TaskActivatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                ActivatedAt = DateTime.UtcNow
            });
        }

        public void Deactivate()
        {
            IsActive = false;
            
            AddDistributedEvent(new TaskDeactivatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                DeactivatedAt = DateTime.UtcNow
            });
        }

        public void UpdateTitle(string newTitle)
        {
            var oldTitle = Title;
            Title = Check.NotNullOrWhiteSpace(newTitle, nameof(newTitle));
            Version++;
            
            AddDistributedEvent(new TaskTitleUpdatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldTitle = oldTitle,
                NewTitle = newTitle,
                Version = Version,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // === COMPUTED ===
        public bool IsRecurring => RecurrenceRule != null;
        public bool IsInArea => AreaId.HasValue;
        public bool IsInProject => ProjectId.HasValue;
        public bool IsInContext => ContextId.HasValue;
        public bool IsOrganized => IsInArea || IsInProject || IsInContext;
        
        /// <summary>
        /// Gets the effective area for instances (blueprint area or default area)
        /// </summary>
        public Guid? GetEffectiveAreaForInstance()
        {
            return AreaId ?? DefaultAreaId;
        }

        /// <summary>
        /// Gets the effective context for instances
        /// </summary>
        public Guid? GetEffectiveContextForInstance()
        {
            return ContextId ?? DefaultContextId;
        }

        public void SetImportanceUrgency(double newImportance, double newUrgency)
        {
            DefaultImportance = newImportance;
            DefaultUrgency = newUrgency;
        }
    }
}