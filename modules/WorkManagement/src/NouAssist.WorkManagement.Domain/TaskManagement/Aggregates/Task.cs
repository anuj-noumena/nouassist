using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Enums;
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
        public Guid? ProjectId { get; private set; }
        public Guid? TaskTypeId { get; private set; }

        // === CONTENT ===
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public string ExpectedResult { get; internal set; }
        public string Notes { get; internal set; }
        
        // === HIERARCHY ===
        public TaskHierarchy Hierarchy { get; private set; }

        // === GTD ORGANIZATION ===
        public Guid? AreaId { get; private set; } // GTD Area of Responsibility
        public Guid? ContextId { get; private set; } // GTD Context (where/with what)
        
        // === DEFAULTS FOR INSTANCES ===
        public double DefaultImportance { get; private set; }
        public double DefaultUrgency { get; private set; }
        public DateTime? DefaultDueDate {get; private set;}
        public int? DefaultEstimateMinutes { get; private set; }
        public Guid? DefaultWorkflowId { get; private set; }
        public Guid? DefaultAssigneeId { get; private set; }
        public Guid? DefaultAreaId { get; private set; } // Default area for instances
        public Guid? DefaultContextId { get; private set; } // Default context for instances
        public ComplexityEstimation DefaultComplexityEstimation { get; private set; }
        public CognitiveLoad? DefaultCognitiveLoad { get; private set; }

        // === RECURRENCE ===
        public RecurrenceRule RecurrenceRule { get; private set; }

        // === STATE ===
        public bool IsActive { get; private set; }
        public int Version { get; private set; }

        // === CHECKLIST ===
        private readonly List<ChecklistItem> _checklistItems = new();
        public IReadOnlyCollection<ChecklistItem> ChecklistItems => _checklistItems.AsReadOnly();

        // === ACCEPTANCE CRITERIA ===
        private readonly List<AcceptanceCriterion> _acceptanceCriteria = new();
        public IReadOnlyCollection<AcceptanceCriterion> AcceptanceCriteria => 
            _acceptanceCriteria.AsReadOnly();

        // === ACTION STEPS ===
        private readonly List<TaskActionStep> _actionSteps = new();
        public IReadOnlyCollection<TaskActionStep> ActionSteps => 
            _actionSteps.AsReadOnly();
        
        protected Task() { }

        public Task(
            Guid id,
            Guid ownerId,
            string title,
            Guid taskTypeId,
            Guid? tenantId = null,
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            TaskHierarchy? taskHierarchy = null,
            double defaultImportance = 0.5,
            double defaultUrgency = 0.5) : base(id)
        {
            OwnerId = ownerId;
            TaskTypeId = taskTypeId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            TenantId = tenantId;
            ProjectId = projectId;
            AreaId = areaId;
            ContextId = contextId;
            
            Hierarchy = taskHierarchy ?? TaskHierarchy.Root();
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

        public void UpdateHirearchy(TaskHierarchy taskHirearchy)
        {
            Hierarchy = taskHirearchy;
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

        // === DEFAULTS UPDATE METHODS ===

        /// <summary>
        /// Updates default priority (importance and urgency) for instances
        /// </summary>
        public void UpdateDefaultPriority(double importance, double urgency)
        {
            var oldImportance = DefaultImportance;
            var oldUrgency = DefaultUrgency;

            DefaultImportance = ImportanceUrgencyMatrix.ValidateImportance(importance);
            DefaultUrgency = ImportanceUrgencyMatrix.ValidateUrgency(urgency);
            Version++;

            AddDistributedEvent(new TaskDefaultPriorityUpdatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldImportance = oldImportance,
                NewImportance = DefaultImportance,
                OldUrgency = oldUrgency,
                NewUrgency = DefaultUrgency,
                Version = Version,
                UpdatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Updates default schedule (due date and estimate) for instances
        /// </summary>
        public void UpdateDefaultSchedule(DateTime? defaultDueDate, int? defaultEstimateMinutes)
        {
            var oldDueDate = DefaultDueDate;
            var oldEstimate = DefaultEstimateMinutes;

            DefaultDueDate = defaultDueDate;
            DefaultEstimateMinutes = defaultEstimateMinutes;
            Version++;

            AddDistributedEvent(new TaskDefaultScheduleUpdatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldDefaultDueDate = oldDueDate,
                NewDefaultDueDate = defaultDueDate,
                OldDefaultEstimateMinutes = oldEstimate,
                NewDefaultEstimateMinutes = defaultEstimateMinutes,
                Version = Version,
                UpdatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Updates default assignee for instances
        /// </summary>
        public void UpdateDefaultAssignee(Guid? defaultAssigneeId)
        {
            var oldAssigneeId = DefaultAssigneeId;
            DefaultAssigneeId = defaultAssigneeId;
            Version++;

            AddDistributedEvent(new TaskDefaultAssigneeUpdatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldDefaultAssigneeId = oldAssigneeId,
                NewDefaultAssigneeId = defaultAssigneeId,
                Version = Version,
                UpdatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Updates default workflow for instances
        /// </summary>
        public void UpdateDefaultWorkflow(Guid? defaultWorkflowId)
        {
            var oldWorkflowId = DefaultWorkflowId;
            DefaultWorkflowId = defaultWorkflowId;
            Version++;

            AddDistributedEvent(new TaskDefaultWorkflowUpdatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldDefaultWorkflowId = oldWorkflowId,
                NewDefaultWorkflowId = defaultWorkflowId,
                Version = Version,
                UpdatedAt = DateTime.UtcNow
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
        /// Sets default complexity estimation for instances created from this blueprint
        /// </summary>
        public void SetDefaultComplexityEstimation(ComplexityEstimation estimation)
        {
            DefaultComplexityEstimation = estimation;
            Version++;
        }

        /// <summary>
        /// Clears default complexity estimation
        /// </summary>
        public void ClearDefaultComplexityEstimation()
        {
            DefaultComplexityEstimation = null;
            Version++;
        }

        /// <summary>
        /// Sets default cognitive load for instances created from this blueprint
        /// </summary>
        public void SetDefaultCognitiveLoad(CognitiveLoad cognitiveLoad)
        {
            DefaultCognitiveLoad = cognitiveLoad;
            Version++;
        }

        /// <summary>
        /// Clears default cognitive load
        /// </summary>
        public void ClearDefaultCognitiveLoad()
        {
            DefaultCognitiveLoad = null;
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

        // === CHECKLIST MANAGEMENT ===
        public void AddChecklistItem(string title, bool isRequired = false)
        {
            var maxOrder = _checklistItems.Any() 
                ? _checklistItems.Max(i => i.OrderIndex) 
                : -1;

            var item = new ChecklistItem(
                Guid.NewGuid(),
                Id,
                title,
                maxOrder + 1,
                isRequired);

            _checklistItems.Add(item);
            Version++;
            
            AddDistributedEvent(new TaskChecklistItemAddedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                ChecklistItemId = item.Id,
                ItemTitle = title,
                IsRequired = isRequired,
                OrderIndex = maxOrder + 1,
                Version = Version,
                AddedAt = DateTime.UtcNow
            });
        }

        public void RemoveChecklistItem(Guid itemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _checklistItems.Remove(item);
                ReorderChecklistItems();
                Version++;
                
                AddDistributedEvent(new TaskChecklistItemRemovedEto
                {
                    TaskId = Id,
                    TenantId = TenantId,
                    ChecklistItemId = itemId,
                    Version = Version,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        public void ReorderChecklistItems(List<Guid> orderedIds)
        {
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = _checklistItems.FirstOrDefault(ci => ci.Id == orderedIds[i]);
                item?.UpdateOrder(i);
            }
            Version++;
        }

        private void ReorderChecklistItems()
        {
            var items = _checklistItems.OrderBy(i => i.OrderIndex).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].UpdateOrder(i);
            }
        }
        
        // === ACTION STEPS MANAGEMENT ===
        public void AddActionStep(string title, string? notes = null, bool? isRequired = false)
        {
            var maxOrder = _actionSteps.Any() 
                ? _actionSteps.Max(i => i.Sequence) 
                : -1;

            var item = new TaskActionStep(
                Guid.NewGuid(),
                Id,
                title,
                notes,
                isRequired ?? false,
                maxOrder + 1);

            _actionSteps.Add(item);
            Version++;
            
            AddDistributedEvent(new TaskActionStepItemAddedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                ActionStepId = item.Id,
                ActionStepTitle = title,
                IsRequired = isRequired ?? false,
                Sequence = maxOrder + 1,
                Version = Version,
                AddedAt = DateTime.UtcNow
            });
        }

        public void RemoveActionStep(Guid itemId)
        {
            var item = _actionSteps.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _actionSteps.Remove(item);
                ReorderChecklistItems();
                Version++;
                
                AddDistributedEvent(new TaskActionStepItemRemovedEto
                {
                    TaskId = Id,
                    TenantId = TenantId,
                    ActionStepId = itemId,
                    Version = Version,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        public void ReorderActionSteps(List<Guid> orderedIds)
        {
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = _actionSteps.FirstOrDefault(ci => ci.Id == orderedIds[i]);
                item?.UpdateSequence(i);
            }
            Version++;
        }

        private void ReorderActionSteps()
        {
            var items = _actionSteps.OrderBy(i => i.Sequence).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].UpdateSequence(i);
            }
        }

        // === ACCEPTANCE CRITERIA ===
        public void AddAcceptanceCriterion(string description, int? priority = null)
        {
            var criterion = new AcceptanceCriterion(Id, description, priority);
            _acceptanceCriteria.Add(criterion);
            Version++;
        }

        public void RemoveAcceptanceCriterion(Guid criterionId)
        {
            var criterion = _acceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion != null)
            {
                _acceptanceCriteria.Remove(criterion);
                Version++;
            }
        }

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
        
        /// <summary>
        /// Updates the task type
        /// </summary>
        public void UpdateTaskType(Guid? newTaskTypeId)
        {
            var oldTaskTypeId = TaskTypeId;
            TaskTypeId = newTaskTypeId;
            Version++;

            AddDistributedEvent(new TaskTypeUpdatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                OldTaskTypeId = oldTaskTypeId,
                NewTaskTypeId = newTaskTypeId,
                Version = Version,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // === CONTENT UPDATE METHODS ===
        
        /// <summary>
        /// Updates all content fields together
        /// </summary>
        public void UpdateContent(string? title, string? description, string? expectedResult, string? notes)
        {
            if (Title != title && !string.IsNullOrWhiteSpace(title))
            {
                Title = title;
            }

            if (Description != description && description != null)
            {
                Description = description;
            }

            if (ExpectedResult != expectedResult && expectedResult != null)
            {
                ExpectedResult = expectedResult;
            }

            if (Notes != notes && notes != null)
            {
                Notes = notes;
            }
        }

        // === COMPUTED ===
        public bool IsRecurring => RecurrenceRule != null;
        public bool HasChecklist => _checklistItems.Any();
        public bool HasAcceptanceCriteria => _acceptanceCriteria.Any();
        public int RequiredChecklistItemCount => _checklistItems.Count(i => i.IsRequired);
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