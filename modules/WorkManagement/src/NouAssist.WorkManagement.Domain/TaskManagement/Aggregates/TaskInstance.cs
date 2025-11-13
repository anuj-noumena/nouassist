using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Shared.ValueObjects;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Events;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// TaskInstance - Runtime execution with GTD Area support
    /// </summary>
    public class TaskInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
    {
        // === CORE IDENTITY ===
        public Guid TaskId { get; private set; }
        public Guid WorkflowId { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int EntityVersion { get; private set; }

        // === OWNERSHIP ===
        public Guid OwnerId { get; private set; }
        public Guid? AssigneeId { get; private set; }
        public Guid? ReporterId { get; private set; }
        public Guid? TeamId { get; set; }

        // === GTD ORGANIZATION ===
        public Guid? AreaId { get; private set; } // GTD Area of Responsibility
        public Guid? ContextId { get; private set; } // GTD Context (where/with what)

        // === WORKFLOW STATE ===
        public Guid WorkflowStatusId { get; private set; }
        
        // === WORK TRACKING ===
        public TaskWorkTracking WorkTracking { get; private set; }
        public TaskSchedule Schedule { get; private set; }
        public ImportanceUrgencyMatrix Priority { get; private set; }
        
        // === APPROVAL ===
        public Guid? ApprovalRequestId { get; private set; }
        
        // === HIERARCHY ===
        public TaskHierarchy Hierarchy { get; private set; }
        
        // === COLLABORATION ===
        public TaskCollaboration Collaboration { get; private set; }
        
        // === CUSTOM DATA ===
        public CustomFieldValues CustomFields { get; private set; }
        
        // === BLOCKING/FLAGS ===
        private string _blockingReason;
        private DateTime? _blockedAt;
        public bool IsBlocked => !string.IsNullOrEmpty(_blockingReason);
        
        private string _flagReason;
        public bool IsFlagged => !string.IsNullOrEmpty(_flagReason);
        
        // === COMPLETION ===
        private string _resolution;
        private string _resolutionNote;
        public bool IsResolved => !string.IsNullOrEmpty(_resolution);

        // === CHILD COLLECTIONS ===
        private readonly List<TaskChecklistItem> _checklistItems = new();
        public IReadOnlyCollection<TaskChecklistItem> ChecklistItems => _checklistItems.AsReadOnly();

        private readonly List<TaskDependency> _dependencies = new();
        public IReadOnlyCollection<TaskDependency> Dependencies => _dependencies.AsReadOnly();

        // === CONSTRUCTION ===
        protected TaskInstance() { }

        public TaskInstance(
            Guid id,
            Guid taskId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid ownerId,
            Guid? projectId,
            Guid? areaId,
            Guid? contextId,
            double importance,
            double urgency,
            Guid? tenantId = null,
            Guid? reporterId = null,
            Guid? assigneeId = null) : base(id)
        {
            TaskId = taskId;
            WorkflowId = workflowId;
            WorkflowStatusId = workflowStatusId;
            OwnerId = ownerId;
            ProjectId = projectId;
            AreaId = areaId;
            ContextId = contextId;
            ReporterId = reporterId ?? ownerId;
            AssigneeId = assigneeId;
            TenantId = tenantId;
            EntityVersion = 1;

            // Validate: cannot be in both project and area
            ValidateProjectAreaExclusivity();

            // Initialize value objects
            Priority = new ImportanceUrgencyMatrix(importance, urgency);
            WorkTracking = TaskWorkTracking.NotStarted();
            Schedule = TaskSchedule.Empty();
            Hierarchy = TaskHierarchy.Root();
            Collaboration = TaskCollaboration.Empty();
            CustomFields = CustomFieldValues.Create();

            AddDistributedEvent(new TaskInstanceCreatedEto
            {
                TaskInstanceId = Id,
                TaskId = TaskId,
                OwnerId = OwnerId,
                ProjectId = ProjectId,
                AreaId = AreaId,
                TenantId = TenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // === GTD ORGANIZATION METHODS ===

        /// <summary>
        /// Assigns this task instance to a GTD Area
        /// </summary>
        public void AssignToArea(Guid areaId)
        {
            if (ProjectId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea",
                    "Task instance cannot belong to both a Project and an Area.");
            }

            var oldAreaId = AreaId;
            AreaId = areaId;
            
            AddDistributedEvent(new TaskInstanceAreaChangedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                OldAreaId = oldAreaId,
                NewAreaId = areaId,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes this task instance from its area
        /// </summary>
        public void RemoveFromArea()
        {
            if (!AreaId.HasValue)
            {
                return;
            }

            var oldAreaId = AreaId;
            AreaId = null;
            
            AddDistributedEvent(new TaskInstanceAreaChangedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                OldAreaId = oldAreaId,
                NewAreaId = null,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Assigns this task instance to a GTD Context
        /// </summary>
        public void AssignToContext(Guid contextId)
        {
            var oldContextId = ContextId;
            ContextId = contextId;
            
            AddDistributedEvent(new TaskInstanceContextChangedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                OldContextId = oldContextId,
                NewContextId = contextId,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes this task instance from its context
        /// </summary>
        public void RemoveFromContext()
        {
            if (!ContextId.HasValue)
            {
                return;
            }

            var oldContextId = ContextId;
            ContextId = null;
            
            AddDistributedEvent(new TaskInstanceContextChangedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                OldContextId = oldContextId,
                NewContextId = null,
                ChangedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Updates GTD organization (area and context together)
        /// </summary>
        public void UpdateOrganization(Guid? newAreaId, Guid? newContextId)
        {
            // Validate: if moving to project, cannot have area
            if (ProjectId.HasValue && newAreaId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea");
            }

            if (AreaId != newAreaId)
            {
                var oldAreaId = AreaId;
                AreaId = newAreaId;
                AddDistributedEvent(new TaskInstanceAreaChangedEto
                {
                    TaskInstanceId = Id,
                    TenantId = TenantId,
                    OldAreaId = oldAreaId,
                    NewAreaId = newAreaId,
                    ChangedAt = DateTime.UtcNow
                });
            }

            if (ContextId != newContextId)
            {
                var oldContextId = ContextId;
                ContextId = newContextId;
                AddDistributedEvent(new TaskInstanceContextChangedEto
                {
                    TaskInstanceId = Id,
                    TenantId = TenantId,
                    OldContextId = oldContextId,
                    NewContextId = newContextId,
                    ChangedAt = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Moves task from area to project
        /// </summary>
        public void MoveToProject(Guid projectId)
        {
            var previousAreaId = AreaId;
            
            if (AreaId.HasValue)
            {
                RemoveFromArea();
            }
            
            ProjectId = projectId;
            
            AddDistributedEvent(new TaskInstanceMovedToProjectEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                ProjectId = projectId,
                PreviousAreaId = previousAreaId,
                MovedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Moves task from project to area
        /// </summary>
        public void MoveToArea(Guid areaId)
        {
            if (ProjectId.HasValue)
            {
                var oldProjectId = ProjectId;
                ProjectId = null;
                
                AddDistributedEvent(new TaskInstanceRemovedFromProjectEto
                {
                    TaskInstanceId = Id,
                    TenantId = TenantId,
                    ProjectId = oldProjectId.Value,
                    RemovedAt = DateTime.UtcNow
                });
            }
            
            AssignToArea(areaId);
        }

        private void ValidateProjectAreaExclusivity()
        {
            if (ProjectId.HasValue && AreaId.HasValue)
            {
                throw new BusinessException("Tasks:CannotBeInBothProjectAndArea",
                    "Task instance cannot belong to both a Project and an Area.");
            }
        }

        // === WORKFLOW TRANSITIONS ===
        public void TransitionTo(Guid newWorkflowStatusId, string comment = null)
        {
            var oldStatusId = WorkflowStatusId;
            WorkflowStatusId = newWorkflowStatusId;
            
            AddDistributedEvent(new TaskStatusTransitionedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                OldStatusId = oldStatusId,
                NewStatusId = newWorkflowStatusId,
                Comment = comment,
                TransitionedAt = DateTime.UtcNow
            });
        }

        // === ASSIGNMENT ===
        public void Assign(Guid assigneeId)
        {
            var oldAssignee = AssigneeId;
            AssigneeId = assigneeId;
            
            if (oldAssignee != assigneeId)
            {
                AddDistributedEvent(new TaskAssignedEto
                {
                    TaskInstanceId = Id,
                    TenantId = TenantId,
                    AssignedTo = assigneeId,
                    PreviousAssignee = oldAssignee,
                    AssignedAt = DateTime.UtcNow
                });
            }
        }

        public void Unassign()
        {
            var oldAssignee = AssigneeId;
            AssigneeId = null;
            
            if (oldAssignee.HasValue)
            {
                AddDistributedEvent(new TaskUnassignedEto
                {
                    TaskInstanceId = Id,
                    TenantId = TenantId,
                    PreviousAssignee = oldAssignee.Value,
                    UnassignedAt = DateTime.UtcNow
                });
            }
        }

        // === WORK TRACKING ===
        public void Start()
        {
            if (WorkTracking.HasStarted)
            {
                throw new BusinessException("Tasks:AlreadyStarted");
            }

            WorkTracking = WorkTracking.Start();
            Schedule = Schedule.MarkStarted(DateTime.UtcNow);
            
            AddDistributedEvent(new TaskStartedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                AssigneeId = AssigneeId,
                StartedAt = DateTime.UtcNow
            });
        }

        public void Complete(string resolution, string resolutionNote = null)
        {
            if (WorkTracking.IsCompleted)
            {
                throw new BusinessException("Tasks:AlreadyCompleted");
            }

            _resolution = Check.NotNullOrWhiteSpace(resolution, nameof(resolution));
            _resolutionNote = resolutionNote;
            
            WorkTracking = WorkTracking.Complete();
            Schedule = Schedule.MarkCompleted(DateTime.UtcNow);
            
            AddDistributedEvent(new TaskCompletedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                Resolution = resolution,
                ResolutionNote = resolutionNote,
                CompletedBy = AssigneeId,
                CompletedAt = DateTime.UtcNow
            });
        }

        public void UpdateProgress(double progressPercent)
        {
            WorkTracking = WorkTracking.UpdateProgress(progressPercent);
        }

        // === APPROVAL ===
        public void LinkApprovalRequest(Guid approvalRequestId)
        {
            ApprovalRequestId = approvalRequestId;
            
            AddDistributedEvent(new TaskApprovalLinkedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                ApprovalRequestId = approvalRequestId,
                LinkedAt = DateTime.UtcNow
            });
        }

        public void ClearApprovalLink()
        {
            ApprovalRequestId = null;
        }

        // === BLOCKING ===
        public void Block(string reason)
        {
            _blockingReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));
            _blockedAt = DateTime.UtcNow;
            
            AddDistributedEvent(new TaskBlockedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                Reason = reason,
                BlockedAt = DateTime.UtcNow
            });
        }

        public void Unblock()
        {
            if (!IsBlocked)
            {
                return;
            }

            _blockingReason = null;
            _blockedAt = null;
            
            AddDistributedEvent(new TaskUnblockedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                UnblockedAt = DateTime.UtcNow
            });
        }

        // === FLAGGING ===
        public void Flag(string reason)
        {
            _flagReason = reason;
            
            AddDistributedEvent(new TaskFlaggedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                Reason = reason,
                FlaggedAt = DateTime.UtcNow
            });
        }

        public void Unflag()
        {
            _flagReason = null;
        }

        // === CHECKLIST ===
        public void InitializeChecklist(List<Guid> checklistItemIds)
        {
            foreach (var itemId in checklistItemIds)
            {
                _checklistItems.Add(new TaskChecklistItem(Id, itemId));
            }
        }

        public void CheckItem(Guid itemId, Guid? userId = null)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == itemId);
            if (item == null)
            {
                throw new BusinessException("Tasks:ChecklistItemNotFound");
            }

            item.Check(userId);
            UpdateProgressFromChecklist();
        }

        public void UncheckItem(Guid itemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == itemId);
            if (item == null)
            {
                throw new BusinessException("Tasks:ChecklistItemNotFound");
            }

            item.Uncheck();
            UpdateProgressFromChecklist();
        }

        private void UpdateProgressFromChecklist()
        {
            if (_checklistItems.Count == 0)
            {
                return;
            }

            var checkedCount = _checklistItems.Count(i => i.IsChecked);
            var progress = (double)checkedCount / _checklistItems.Count * 100.0;
            
            WorkTracking = WorkTracking.UpdateProgress(progress);
        }

        // === DEPENDENCIES ===
        public void AddDependency(
            Guid dependsOnTaskInstanceId, 
            DependencyType type,
            int lagMinutes = 0,
            bool isBlocking = true)
        {
            if (dependsOnTaskInstanceId == Id)
            {
                throw new BusinessException("Tasks:CannotDependOnSelf");
            }

            if (_dependencies.Any(d => d.DependsOnTaskInstanceId == dependsOnTaskInstanceId))
            {
                throw new BusinessException("Tasks:DependencyAlreadyExists");
            }

            var dependency = TaskDependency.CreateInstanceDependency(
                Id, dependsOnTaskInstanceId, type, lagMinutes, isBlocking);
            
            _dependencies.Add(dependency);
        }

        public void RemoveDependency(Guid dependsOnTaskInstanceId)
        {
            var dependency = _dependencies.FirstOrDefault(d => 
                d.DependsOnTaskInstanceId == dependsOnTaskInstanceId);
            
            if (dependency != null)
            {
                _dependencies.Remove(dependency);
            }
        }

        // === COLLABORATION ===
        public void AddWatcher(Guid userId)
        {
            Collaboration = Collaboration.AddWatcher(userId);
        }

        public void RemoveWatcher(Guid userId)
        {
            Collaboration = Collaboration.RemoveWatcher(userId);
        }

        public void IncrementComments()
        {
            Collaboration = Collaboration.IncrementComments();
        }

        public void IncrementAttachments()
        {
            Collaboration = Collaboration.IncrementAttachments();
        }

        // === CUSTOM FIELDS ===
        public void SetCustomFieldValue(string key, object value)
        {
            CustomFields = CustomFields.SetValue(key, value);
        }

        // === TIME TRACKING ===
        public void LogTime(int minutes)
        {
            if (minutes <= 0)
            {
                throw new BusinessException("Tasks:InvalidTimeLog");
            }

            WorkTracking = WorkTracking.LogTime(minutes);
        }

        // === SCHEDULE ===
        public void SetPlannedDates(DateTime? start, DateTime? due)
        {
            Schedule = Schedule.WithPlannedDates(start, due);
        }

        public void SetEstimate(int minutes)
        {
            WorkTracking = WorkTracking.WithEstimate(minutes);
        }

        // === PRIORITY ===
        public void UpdatePriority(double importance, double urgency)
        {
            Priority = new ImportanceUrgencyMatrix(importance, urgency);
        }

        // === COMPUTED PROPERTIES ===
        public bool RequiresApproval => ApprovalRequestId.HasValue;
        public bool HasBlockingDependencies => _dependencies.Any(d => d.IsBlocking);
        public double CompletionRate => WorkTracking.ProgressPercent;
        public bool IsOverdue => Schedule.IsOverdue;
        public bool HasWatchers => Collaboration.WatcherCount > 0;
        
        // GTD-specific computed properties
        public bool IsInArea => AreaId.HasValue;
        public bool IsInProject => ProjectId.HasValue;
        public bool IsInContext => ContextId.HasValue;
        public bool IsOrganized => IsInArea || IsInProject || IsInContext;
        public bool IsAreaTask => IsInArea && !IsInProject;
        public bool IsProjectTask => IsInProject && !IsInArea;
    }
}