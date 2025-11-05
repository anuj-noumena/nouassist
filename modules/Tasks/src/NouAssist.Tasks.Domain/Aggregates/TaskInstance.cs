using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Tasks.Entities;
using NouAssist.Tasks.Enums;
using NouAssist.Tasks.Events;
using NouAssist.Tasks.Policies;
using NouAssist.Tasks.ValueObjects;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Tasks.Aggregates
{
    /// <summary>
    /// TaskInstance - Runtime execution of a Task blueprint
    /// </summary>
    public class TaskInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
    {
        public Guid TaskId { get; private set; }
        public Guid ProjectId { get; private set; }

        public Guid OwnerId { get; private set; }
        public Guid? AssigneeId { get; private set; }
        public Guid? ReporterId { get; private set; }
        public Guid? TeamId { get; set; }
        public Guid? TenantId { get; private set; }

        // Approval
        public TaskApproval Approval { get; internal set; }

        // Status
        
        public TaskProgress Progress { get; internal set; }

        // Importance/Urgency
        public ImportanceUrgencyMatrix PriorityMatrix { get; private set; }

        // Delegation
        public DelegationInfo? DelegationInfo { get; private set; }

        // Dates
        public TaskSchedule Schedule { get; internal set; }

        // Time Tracking
        public TimeTrackingInfo TimeTracking { get; private set; } = TimeTrackingInfo.None;

        // Hierarchy
        public TaskHierarchy Hierarchy { get; internal set; }

        // Collaboration
        public TaskDiscussion Discussion { get; internal set; }

        public BlockingInfo BlockingInfo { get; private set; } = BlockingInfo.NotBlocked;

        public FlagInfo FlagInfo { get; private set; } = FlagInfo.NotFlagged;

        // Resolution
        public ResolutionInfo Resolution { get; private set; } = ResolutionInfo.Unresolved;

        // Custom Fields & Labels
        public Dictionary<string, object> CustomFieldValues { get; private set; }

        
        public int EntityVersion { get; }

        // Child entities
        private readonly List<TaskActionStepCompletion> _stepCompletions = new();

        public IReadOnlyCollection<TaskActionStepCompletion> StepCompletions =>
            _stepCompletions.AsReadOnly();

        private readonly List<AcceptanceCriterionCompletion> _criterionCompletions = new();

        public IReadOnlyCollection<AcceptanceCriterionCompletion> CriterionCompletions =>
            _criterionCompletions.AsReadOnly();

        private readonly List<TaskDependency> _dependencies = new();
        public IReadOnlyCollection<TaskDependency> Dependencies => _dependencies.AsReadOnly();

        protected TaskInstance()
        {
        }

        public TaskInstance(
            Guid id,
            Guid taskId,
            Guid projectId,
            Guid workflowStatusId,
            Guid ownerId,
            double importance,
            double urgency,
            Guid? tenantId = null,
            Guid? assigneeId = null) : base(id)
        {
            TaskId = Check.NotNull(taskId, nameof(taskId));
            ProjectId = Check.NotNull(projectId, nameof(projectId));
            WorkflowStatusId = Check.NotNull(workflowStatusId, nameof(workflowStatusId));
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            AssigneeId = assigneeId;
            TenantId = tenantId;

            PriorityMatrix = new ImportanceUrgencyMatrix(importance, urgency);
            TaskStatus = WorkflowStatusCategory.Todo;
            ProgressPercent = 0.0;
            CustomFieldValues = new Dictionary<string, object>();

            AddDistributedEvent(new TaskInstanceCreatedEto
            {
                TaskInstanceId = Id,
                TaskId = TaskId,
                TenantId = TenantId,
                OwnerId = OwnerId,
                ProjectId = ProjectId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Properties
        
        public bool IsDelegated => DelegationInfo != null;

        public bool IsWaitingOnDelegation => IsDelegated &&
                                             DelegationInfo?.Status == DelegationStatus.Delegated;

        public bool IsCancelled => TaskStatus == WorkflowStatusCategory.Cancelled;
        public bool IsInProgress => TaskStatus == WorkflowStatusCategory.InProgress;
        
        // State Transitions
        public void Start(ITaskStartPolicy startPolicy, Guid startedBy)
        {
            var policyCheck = startPolicy.Check(this);
            if (!policyCheck.CanStart)
            {
                throw new BusinessException(policyCheck.FailureReason);
            }

            var startDate = DateTime.UtcNow;
            Schedule = Schedule.WithActualDates(startDate, Schedule.PlannedDueDate);
            TaskStatus = WorkflowStatusCategory.InProgress;

            AddDistributedEvent(new TaskStartedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                StartedBy = startedBy,
                StartedAt = startDate
            });
        }

        public void Assign(Guid assigneeId, Guid assignedBy)
        {
            AssigneeId = Check.NotNull(assigneeId, nameof(assigneeId));

            AddDistributedEvent(new TaskAssignedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                AssignedTo = assigneeId,
                AssignedBy = assignedBy,
                AssignedAt = DateTime.UtcNow
            });
        }

        public void Unassign()
        {
            AssigneeId = null;

            AddDistributedEvent(new TaskUnassignedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
            });
        }

        public void ResetStatus()
        {
            TaskStatus = WorkflowStatusCategory.InProgress;
            Schedule = Schedule.MarkIncompleted();
            Resolution = ResolutionInfo.Unresolved; // Use the default state
        }

        public void Complete(Guid completedBy, ITaskCompletionPolicy completionPolicy, string resolution = null,
            string resolutionNote = null)
        {
            // Delegate the rule check to the policy
            var policyCheck = completionPolicy.Check(this);
            if (!policyCheck.CanComplete)
            {
                throw new BusinessException(policyCheck.FailureReason);
            }
            var completedAt = DateTime.UtcNow;
            Schedule = Schedule.MarkCompleted(completedAt);
            
            TaskStatus = WorkflowStatusCategory.Done;
            ProgressPercent = 100.0;
            Resolution = ResolutionInfo.Create(resolution, resolutionNote);
            
            AddDistributedEvent(new TaskCompletedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                CompletedBy = completedBy,
                CompletedAt = completedAt,
                ActualMinutes = TimeTracking.ActualMinutes,
                Resolution = Resolution.Resolution
            });
        }

        public void Cancel(string reason)
        {
            if (Schedule.IsCompleted)
            {
                throw new BusinessException("Tasks:CannotCancelCompleted");
            }

            TaskStatus = WorkflowStatusCategory.Cancelled;
            Resolution = ResolutionInfo.Create("Cancelled", reason);
        }

        // Delegation
        public void Delegate(Guid delegatedBy, Guid delegatedTo, string note = null)
        {
            if (delegatedBy == delegatedTo)
            {
                throw new BusinessException("Tasks:CannotDelegateToSelf");
            }

            if (IsDelegated)
            {
                throw new BusinessException("Tasks:AlreadyDelegated");
            }

            DelegationInfo = new DelegationInfo(delegatedBy, delegatedTo, note);
            AssigneeId = delegatedTo;

            AddDistributedEvent(new TaskDelegatedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                DelegatedBy = delegatedBy,
                DelegatedTo = delegatedTo,
                DelegatedAt = DateTime.UtcNow,
                Note = note
            });
        }

        public void AcceptDelegation(Guid acceptedBy, string responseNote = null)
        {
            if (!IsDelegated)
            {
                throw new BusinessException("Tasks:NotDelegated");
            }

            if (DelegationInfo.DelegatedTo != acceptedBy)
            {
                throw new BusinessException("Tasks:OnlyDelegateCanAccept");
            }

            DelegationInfo = DelegationInfo.Accept(responseNote);
            TaskStatus = WorkflowStatusCategory.InProgress;
            

            AddDistributedEvent(new TaskDelegationAcceptedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                AcceptedBy = acceptedBy,
                AcceptedAt = DateTime.UtcNow,
                ResponseNote = responseNote
            });
        }

        public void DeclineDelegation(Guid declinedBy, string responseNote)
        {
            if (!IsDelegated)
            {
                throw new BusinessException("Tasks:NotDelegated");
            }

            if (DelegationInfo.DelegatedTo != declinedBy)
            {
                throw new BusinessException("Tasks:OnlyDelegateCanDecline");
            }

            DelegationInfo = DelegationInfo.Decline(responseNote);
            AssigneeId = null;
            

            AddDistributedEvent(new TaskDelegationDeclinedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                DeclinedBy = declinedBy,
                DeclinedAt = DateTime.UtcNow,
                Reason = responseNote
            });
        }


        // Blocking
        public void Block(string reason)
        {
            BlockingInfo = BlockingInfo.Create(reason); // Assumes VO handles validation
            

            AddDistributedEvent(new TaskBlockedEto
            {
                TaskInstanceId = Id,
                TenantId = TenantId,
                Reason = reason,
                BlockedAt = BlockingInfo.BlockedAt.Value
            });
        }

        public void BlockWaitingForUser(string userName, string additionalContext = null)
        {
            var reason = additionalContext != null
                ? $"Waiting for: {userName} - {additionalContext}"
                : $"Waiting for: {userName}";
            Block(reason);
        }

        public void Unblock()
        {
            if (!BlockingInfo.IsBlocked)
            {
                throw new BusinessException("Tasks:NotBlocked");
            }

            BlockingInfo = BlockingInfo.NotBlocked;
            
        }

        // Step Completions
        public void InitializeStepCompletions(List<Guid> actionStepIds)
        {
            foreach (var stepId in actionStepIds)
            {
                _stepCompletions.Add(new TaskActionStepCompletion(Id, stepId));
            }
        }

        public void CompleteStep(Guid stepId, Guid completedBy)
        {
            var completion = _stepCompletions.FirstOrDefault(sc => sc.TaskActionStepId == stepId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:StepCompletionNotFound");
            }

            completion.Complete(completedBy);
            UpdateProgress();
            
        }

        public void UncompleteStep(Guid stepId)
        {
            var completion = _stepCompletions.FirstOrDefault(sc => sc.TaskActionStepId == stepId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:StepCompletionNotFound");
            }

            completion.Uncomplete();
            UpdateProgress();
            
        }

        // Criterion Completions
        public void InitializeCriterionCompletions(List<Guid> criterionIds)
        {
            foreach (var criterionId in criterionIds)
            {
                _criterionCompletions.Add(new AcceptanceCriterionCompletion(Id, criterionId));
            }
        }

        public void MarkCriterionAsMet(Guid criterionId, Guid metBy)
        {
            var completion = _criterionCompletions.FirstOrDefault(cc => cc.CriterionId == criterionId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:CriterionCompletionNotFound");
            }

            completion.MarkAsMet(metBy);
            
        }

        // Time Tracking
        public void LogTimeSpent(int minutes)
        {
            if (minutes <= 0)
            {
                throw new BusinessException("Tasks:InvalidTimeLog");
            }

            // The VO handles the logic of creating the new state
            TimeTracking = TimeTracking.LogTime(minutes);
            
        }

        // Watchers
        public void AddWatcher(Guid userId)
        {
            if (_watchers.Contains(userId))
            {
                return;
            }

            _watchers.Add(userId);
            
        }

        public void RemoveWatcher(Guid userId)
        {
            _watchers.Remove(userId);
            
        }

        // Progress Calculation
        private void UpdateProgress()
        {
            if (_stepCompletions.Count == 0)
            {
                ProgressPercent = 0.0;
                return;
            }

            var completedCount = _stepCompletions.Count(sc => sc.IsDone);
            ProgressPercent = (double)completedCount / _stepCompletions.Count * 100.0;
        }

        // Dependencies Management
        public void AddDependency(Guid dependsOnTaskId, DependencyType dependencyType,
            int lagMinutes = 0, bool isBlocking = true)
        {
            if (dependsOnTaskId == Id)
            {
                throw new BusinessException("Tasks:CannotDependOnSelf");
            }

            if (_dependencies.Any(d => d.DependsOnTaskId == dependsOnTaskId))
            {
                throw new BusinessException("Tasks:DependencyAlreadyExists");
            }

            var dependency = TaskDependency.CreateTaskDependency(
                Id, dependsOnTaskId, dependencyType, lagMinutes, isBlocking);
            _dependencies.Add(dependency);
            
        }

        public void RemoveDependency(Guid dependsOnTaskId)
        {
            var dependency = _dependencies.FirstOrDefault(d => d.DependsOnTaskId == dependsOnTaskId);
            if (dependency == null)
            {
                throw new BusinessException("Tasks:DependencyNotFound");
            }

            _dependencies.Remove(dependency);
            
        }

        // Other Methods
        public void MarkAsNextAction()
        {
            IsNextAction = true;
            
        }

        public void UnmarkAsNextAction()
        {
            IsNextAction = false;
            
        }

        public void LockDiscussion()
        {
            IsDiscussionLocked = true;
            
        }

        public void UnlockDiscussion()
        {
            IsDiscussionLocked = false;
            
        }

        public void IncrementCommentsCount()
        {
            CommentsCount++;
        }

        public void IncrementAttachmentsCount()
        {
            AttachmentsCount++;
        }

        public void UpdatePriority(double importance, double urgency)
        {
            PriorityMatrix = new ImportanceUrgencyMatrix(importance, urgency);
            
        }

        public void UpdateFlag(bool isFlagged, string flaggedReason)
        {
            // The logic is now encapsulated in the VO's factories
            FlagInfo = isFlagged
                ? FlagInfo.Create(flaggedReason)
                : FlagInfo.NotFlagged;
            
        }

        public void UpdateWorkflowStatus(Guid workflowStatusId)
        {
            WorkflowStatusId = workflowStatusId;
        }

        public void RevokeDelegation()
        {
            DelegationInfo = null;
            AssigneeId = OwnerId;
        }
        
        public void MoveToProject(Guid projectId, Guid workflowStatusId)
        {
            ProjectId = projectId;
            WorkflowStatusId = workflowStatusId;
        }
        
    }
}