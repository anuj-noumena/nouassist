# Work Management System - Domain Model Documentation

## Table of Contents
1. [System Overview](#system-overview)
2. [Bounded Contexts](#bounded-contexts)
3. [Approval Context](#approval-context)
4. [Project Management Context](#project-management-context)
5. [Task Management Context](#task-management-context)
6. [Workflow Context](#workflow-context)
7. [Domain Events](#domain-events)
8. [Cross-Context Integration](#cross-context-integration)

---

## System Overview

The Work Management System is a comprehensive domain-driven design (DDD) implementation built on ABP Framework. It manages the complete lifecycle of tasks, projects, and approval workflows within an organization.

### Core Principles
- **Multi-tenancy**: Full tenant isolation across all modules
- **Audit Trail**: Complete tracking of all changes via FullAuditedEntity
- **Event-Driven Architecture**: Domain events for cross-context communication
- **Separation of Concerns**: Clear bounded contexts with well-defined interfaces

---

## Bounded Contexts

### 1. Approval Context
**Purpose**: Manages multi-step approval workflows with configurable rules and policies.

### 2. Project Management Context
**Purpose**: Handles project planning, execution, tracking, and success criteria management.

### 3. Task Management Context
**Purpose**: Manages task blueprints and their runtime instances with full lifecycle support.

### 4. Workflow Context
**Purpose**: Provides flexible status workflows with transitions, conditions, and automated actions.

---

## Approval Context

### Aggregates

#### **ApprovalWorkflow**
**Root Entity**: `ApprovalWorkflow`
**Responsibility**: Defines approval process structure and execution rules.

**Key Properties**:
```csharp
- Name: string
- WorkflowType: ApprovalWorkflowType
- Strategy: ApprovalStrategy (Sequential, Parallel, Custom)
- IsActive: bool
- AllowReassign: bool
- AllowDelegate: bool
- TimeoutDays: int?
- TimeoutAction: string
- CustomFields: CustomFieldValues
```

**Child Entities**:
- `ApprovalStep`: Individual approval stages with requirements and approvers

**Invariants**:
- Cannot activate without at least one step
- Cannot modify active workflow
- Step order indices must be unique and sequential

**Key Behaviors**:
```csharp
void Activate()
void Deactivate()
ApprovalStep AddStep(name, orderIndex, requirement, description)
void RemoveStep(stepId)
void ReorderSteps()
ApprovalStep GetNextStep(currentStepId)
bool IsLastStep(stepId)
```

---

#### **ApprovalRequest**
**Root Entity**: `ApprovalRequest`
**Responsibility**: Tracks approval request lifecycle and decision history.

**Key Properties**:
```csharp
- WorkflowId: Guid
- EntityType: string
- EntityId: Guid
- RequestedBy: Guid
- Status: ApprovalStatus (Pending, InProgress, Approved, Rejected, Cancelled)
- CurrentStepId: Guid?
- Priority: int (0-10)
- DueDate: DateTime?
- CustomFields: CustomFieldValues
```

**Child Entities**:
- `ApprovalDecision`: Individual approval/rejection decisions

**State Machine**:
```
Pending → InProgress → [Approved | Rejected]
         ↓
      Cancelled
```

**Key Behaviors**:
```csharp
void MoveToStep(stepId)
void CompleteStep(stepId)
void Approve(userId)
void Reject(userId)
void Cancel(cancelledBy, reason)
void AddDecision(decision)
```

**Derived State Properties**:
```csharp
bool IsOverdue
TimeSpan? TimeRemaining
bool IsNearingDeadline(hoursThreshold)
```

---

### Entities

#### **ApprovalStep**
**Parent**: ApprovalWorkflow
**Responsibility**: Defines approval requirements and approver assignments.

**Key Properties**:
```csharp
- Name: string
- OrderIndex: int
- Requirement: ApprovalRequirement (Any, All, Majority, Custom)
- RequiredApprovals: int?
- AllowSelfApproval: bool
- TimeoutHours: int?
- TimeoutAction: string
- ApproverUserIds: IReadOnlyCollection<Guid>
- ApproverRoleIds: IReadOnlyCollection<Guid>
- ApproverTeamIds: IReadOnlyCollection<Guid>
- Conditions: Dictionary<string, object>
```

**Key Behaviors**:
```csharp
void AddApproverUser(userId)
void AddApproverRole(roleId)
void AddApproverTeam(teamId)
bool HasApprovers()
void ValidateApprovers()
bool EvaluateConditions(context)
```

---

#### **ApprovalDecision**
**Parent**: ApprovalRequest
**Responsibility**: Records individual approval decisions with full audit trail.

**Key Properties**:
```csharp
- RequestId: Guid
- StepId: Guid
- ApproverId: Guid
- Action: ApprovalAction (Approved, Rejected, Abstained)
- Comments: string
- Signature: string
- DecidedAt: DateTime
- IsDelegated: bool
- DelegatedFrom: Guid?
```

**Derived Properties**:
```csharp
bool IsApproval
bool IsRejection
bool IsAbstained
```

---

### Domain Services

#### **ApprovalWorkflowEngine**
**Responsibility**: Orchestrates approval workflow execution and decision processing.

**Key Operations**:
```csharp
WorkflowExecutionResult ProcessDecision(
    request, workflow, decision, userId)

ApprovalRequest InitiateApprovalRequest(
    workflow, entityType, entityId, requestedBy, tenantId, customFields)
```

**Business Rules**:
- Validates step completion based on approval requirements
- Handles step-to-step transitions
- Processes rejections (terminates workflow)
- Emits workflow completion events

---

#### **ApprovalPermissionChecker**
**Responsibility**: Validates user permissions for approval actions.

**Key Operations**:
```csharp
bool CanUserApproveStep(
    userId, step, userRoleIds, userTeamIds, requestedBy)

List<Guid> GetEligibleApprovers(
    step, userRoles, userTeams)
```

**Permission Logic**:
- Direct user assignment
- Role-based assignment
- Team-based assignment
- Self-approval validation

---

### Value Objects

#### **ApprovalActionDefinition**
```csharp
- ActionType: ApprovalActionType
- Label: string
- IsUserAction: bool
- IsAutomatic: bool
- ConditionKey: string
```

---

### Specifications

#### **PendingApprovalRequestsSpecification**
```csharp
Status == Pending || Status == InProgress
```

#### **OverdueApprovalRequestsSpecification**
```csharp
DueDate < Now && (Status == Pending || Status == InProgress)
```

#### **ApprovalRequestsByWorkflowSpecification**
```csharp
WorkflowId == {workflowId}
```

---

## Project Management Context

### Aggregates

#### **Project**
**Root Entity**: `Project`
**Responsibility**: Central project planning and execution management.

**Key Properties**:
```csharp
- OwnerId: Guid
- Title: string
- Status: ProjectStatus (Planned, Active, OnHold, Completed, Cancelled)
- IsArchived: bool
- Importance: double (0-1)
- Urgency: double (0-1)
- EisenhowerQuadrant: enum
- StartDate: DateTime?
- DueDate: DateTime?
- Progress: double (0-100)
- ExpectedOutcome: string
```

**Relationships**:
```csharp
- ParentProjectId: Guid?
- GoalId: Guid?
- WorkflowSchemeId: Guid?
- SuccessCriterionIds: IReadOnlyCollection<Guid>
- CollaboratingTeamIds: IReadOnlyCollection<Guid>
```

**State Machine**:
```
Planned → Active → [Completed | Cancelled]
         ↓    ↑
      OnHold──┘
```

**Key Behaviors**:
```csharp
void SetPriority(importance, urgency)
void Start()
void Complete(completedBy)
void Cancel()
void Pause()
void Archive()
void UpdateProgress(newProgress)
void AddSuccessCriterion(criterionId)
void MarkSuccessCriterionMet(index, metBy)
```

**Derived Properties**:
```csharp
bool IsOverdue
TimeSpan? TimeRemaining
ProjectHealth Health  // Healthy, AtRisk, Critical, Completed, Cancelled
bool AllSuccessCriteriaMet
double SuccessCriteriaCompletionRate
```

**Health Calculation Factors**:
1. Progress vs. time elapsed
2. Success criteria completion
3. Overdue status

---

#### **ProjectSprint**
**Root Entity**: `ProjectSprint`
**Responsibility**: Manages time-boxed development iterations.

**Key Properties**:
```csharp
- Name: string
- Goal: string
- StartDate: DateTime
- EndDate: DateTime
- PlannedStoryPoints: int
- CompletedStoryPoints: int
- VelocityEstimate: int
- ActualVelocity: int
- Status: SprintStatus (Planned, InProgress, Completed)
- RetrospectiveNotes: string
```

**Invariants**:
- Duration must be 7-30 days
- EndDate must be after StartDate
- CompletedStoryPoints cannot exceed PlannedStoryPoints

**Key Behaviors**:
```csharp
void SetDates(startDate, endDate)
void Start()
void Complete(completedBy, retrospectiveNotes)
void UpdateStoryPoints(planned, completed)
```

**Derived Properties**:
```csharp
int DurationInDays
bool IsActive
TimeSpan? TimeRemaining
double CompletionRate
double VelocityAccuracy
SprintHealth Health  // OnTrack, AtRisk, BehindSchedule, Completed
```

---

### Entities

#### **ProjectDependency**
**Parent**: Project
**Responsibility**: Manages project-level dependencies and blocking relationships.

**Key Properties**:
```csharp
- ProjectId: Guid
- DependsOnProjectId: Guid
- DependencyType: ProjectDependencyType
- LagDays: int
- IsBlocking: bool
- Status: ProjectDependencyStatus (Active, Resolved, Blocked, Obsolete)
- ExpectedResolutionDate: DateTime?
- ApprovalRequestId: Guid?
```

**Dependency Types**:
- FinishToStart
- StartToStart
- FinishToFinish
- StartToFinish
- Deliverable
- Resource
- Approval
- Information

**Key Behaviors**:
```csharp
void Resolve(resolvedBy)
void MarkAsObsolete()
void Block()
void Reactivate()
```

**Invariants**:
- Project cannot depend on itself
- Circular dependencies are validated

---

#### **ProjectSuccessCriterion**
**Parent**: Project
**Responsibility**: Tracks measurable project success indicators.

**Key Properties**:
```csharp
- Title: string
- CriterionType: SuccessCriterionType (Quantitative, Qualitative, Binary)
- MeasurementMethod: string
- TargetValue: string
- ActualValue: string
- TargetNumeric: double?
- ActualNumeric: double?
- Weight: double (0-10)
- IsMet: bool
- TargetDate: DateTime?
```

**Key Behaviors**:
```csharp
void SetWeight(weight)
void MarkAsMet(metBy, notes, evidenceUrl)
void UnmarkAsMet()
void UpdateActualValue(actualValue, actualNumeric)
```

**Derived Properties**:
```csharp
double? AchievementPercentage
bool IsOverdue
bool HasEvidence
```

---

#### **ProjectRequirement**
**Parent**: Project
**Responsibility**: Manages project requirements with approval workflow.

**Key Properties**:
```csharp
- Title: string
- RequirementType: ProjectRequirementType
- Status: ProjectRequirementStatus 
  (Proposed, Approved, InProgress, Implemented, Verified, Rejected)
- Priority: PriorityLevel
- AcceptanceCriteria: string
- StoryPoints: int?
- BusinessValue: int?
```

**State Machine**:
```
Proposed → Approved → InProgress → Implemented → Verified
   ↓
Rejected
```

---

#### **ProjectEvent**
**Parent**: Project
**Responsibility**: Tracks project timeline events and milestones.

**Key Properties**:
```csharp
- Title: string
- EventType: ProjectEventType
- EventDate: DateTime
- EndDate: DateTime?
- IsAllDay: bool
- Location: string
- IsCompleted: bool
```

---

### Domain Services

#### **ProjectHealthCalculationService**
**Responsibility**: Computes comprehensive project health metrics.

**Output**: `ProjectHealthReport`
```csharp
- BaseHealth: ProjectHealth
- OverallHealth: ProjectHealth
- BlockedDependenciesCount: int
- HasCriticalBlockers: bool
- Recommendations: List<string>
```

---

#### **ProjectDependencyManagementService**
**Responsibility**: Validates and manages project dependency graphs.

**Key Operations**:
```csharp
DependencyValidationResult ValidateDependencyChain(
    projectId, dependsOnProjectId, allDependencies)

CriticalPathResult CalculateCriticalPath(
    dependencies, projectDurations)
```

**Algorithms**:
- Circular dependency detection
- Topological sorting
- Critical path calculation
- Earliest start date computation

---

#### **SprintPlanningService**
**Responsibility**: Assists in sprint planning and capacity management.

**Key Operations**:
```csharp
SprintPlanResult PlanSprint(
    project, startDate, endDate, availableCapacity, backlogItems)

int CalculateTeamVelocity(
    completedSprints, lastNSprints)
```

---

### Value Objects

#### **SprintMetrics**
```csharp
- PlannedStoryPoints: int
- CompletedStoryPoints: int
- VelocityEstimate: int
- ActualVelocity: int
- CompletionRate: double
- VelocityAccuracy: double
```

#### **DateRange**
```csharp
- StartDate: DateTime
- EndDate: DateTime
- DurationInDays: int
- IsActive: bool
- TimeRemaining: TimeSpan?
```

---

## Task Management Context

### Aggregates

#### **Task** (Blueprint)
**Root Entity**: `Task`
**Responsibility**: Template definition for task instances.

**Key Properties**:
```csharp
- OwnerId: Guid
- Title: string
- ProjectId: Guid?
- Description: string
- DefaultImportance: double
- DefaultUrgency: double
- EstimateMinutes: int?
- DefaultDueDate: DateTime?
- IsRecurring: bool
- RecurrenceRule: RecurrenceRule
- IsActive: bool
- Version: int
```

**Child Collections**:
- `ActionSteps`: List<TaskActionStep> (max 50)
- `AcceptanceCriteria`: List<AcceptanceCriterion> (max 20)

**Key Behaviors**:
```csharp
void AddActionStep(title, notes, isRequired, sequence)
void RemoveActionStep(stepId)
void ReorderActionSteps(orderedStepIds)
void AddAcceptanceCriterion(description, priority, given, when, then)
void MakeRecurring(recurrenceRule)
void RemoveRecurrence()
void Activate()
void Deactivate()
void SoftDelete()
```

**Invariants**:
- Maximum 50 action steps
- Maximum 20 acceptance criteria
- Title required and unique within project
- Importance and urgency between 0-1

---

#### **TaskInstance** (Runtime)
**Root Entity**: `TaskInstance`
**Responsibility**: Executable instance with full state tracking.

**Key Value Objects**:
```csharp
- Progress: TaskProgress
- Schedule: TaskSchedule
- PriorityMatrix: ImportanceUrgencyMatrix
- DelegationInfo: DelegationInfo?
- Hierarchy: TaskHierarchy
- Discussion: TaskDiscussion
- BlockingInfo: BlockingInfo
- FlagInfo: FlagInfo
- Resolution: ResolutionInfo
- TimeTracking: TimeTrackingInfo
- Approval: TaskApproval
```

**Child Entities**:
- `StepCompletions`: List<TaskActionStepCompletion>
- `CriterionCompletions`: List<AcceptanceCriterionCompletion>
- `Dependencies`: List<TaskDependency>
- `History`: List<TaskHistoryEntry>

**State Machine** (via WorkflowStatusCategory):
```
Todo → InProgress → Done
  ↓         ↓         ↑
Blocked  Cancelled  (can reopen)
```

**Key Behaviors**:
```csharp
// Lifecycle
void Start(startPolicy, startedBy)
void Complete(completedBy, completionPolicy, resolution, note)
void Cancel(reason)
void ResetStatus()

// Assignment
void Assign(assigneeId, assignedBy)
void Unassign()

// Delegation
void Delegate(delegatedBy, delegatedTo, note)
void AcceptDelegation(acceptedBy, responseNote)
void DeclineDelegation(declinedBy, responseNote)
void RevokeDelegation()

// Blocking
void Block(reason)
void BlockWaitingForUser(userName, context)
void Unblock()

// Progress Tracking
void CompleteStep(stepId, completedBy)
void UncompleteStep(stepId)
void MarkCriterionAsMet(criterionId, metBy)

// Collaboration
void AddWatcher(userId)
void RemoveWatcher(userId)
void IncrementCommentsCount()
void IncrementAttachmentsCount()
void LockDiscussion()
void UnlockDiscussion()

// Dependencies
void AddDependency(dependsOnTaskId, type, lagMinutes, isBlocking)
void RemoveDependency(dependsOnTaskId)

// Metadata
void MarkAsNextAction()
void UpdatePriority(importance, urgency)
void UpdateFlag(isFlagged, reason)
void UpdateWorkflowStatus(statusId)
void UpdateCustomFieldValue(fieldName, value, updatedBy)
```

**Derived Properties**:
```csharp
bool IsDelegated
bool IsWaitingOnDelegation
bool IsOverdue
TimeSpan? TimeRemaining
```

---

#### **TaskType**
**Root Entity**: `TaskType`
**Responsibility**: Categorizes tasks with custom field schemas.

**Key Properties**:
```csharp
- Name: string
- Description: string
- Color: string (hex format)
- IsActive: bool
- CustomFields: IReadOnlyCollection<TaskTypeCustomField>
```

**Key Behaviors**:
```csharp
void AddCustomField(definitionId, isRequired, defaultValue, displayOrder)
void RemoveCustomField(definitionId)
void ReorderCustomFields(orderedFieldIds)
```

---

### Entities

#### **TaskActionStep**
**Parent**: Task
**Responsibility**: Individual step in task execution checklist.

**Key Properties**:
```csharp
- TaskId: Guid
- Title: string
- Notes: string
- IsRequired: bool
- Sequence: int
```

---

#### **AcceptanceCriterion**
**Parent**: Task
**Responsibility**: Defines success conditions using Given-When-Then format.

**Key Properties**:
```csharp
- Description: string
- Priority: int?
- GivenContext: string
- WhenAction: string
- ThenOutcome: string
- TestScenario: string
```

**Behaviors**:
```csharp
bool HasGivenWhenThen
string GetFormattedGWT()
```

---

#### **TaskDependency**
**Composite Entity**: Links tasks/instances
**Responsibility**: Manages task execution dependencies.

**Key Properties**:
```csharp
- TaskId/TaskInstanceId: Guid?
- DependsOnTaskId/DependsOnTaskInstanceId: Guid?
- DependencyType: DependencyType
- LagMinutes: int
- IsBlocking: bool
```

**Dependency Types**:
- FinishToStart
- StartToStart
- FinishToFinish
- StartToFinish

---

#### **TaskHistoryEntry**
**Parent**: TaskInstance
**Responsibility**: Audit trail of all task changes.

**Key Properties**:
```csharp
- ChangeType: TaskHistoryChangeType
- ChangedBy: Guid?
- ChangedAt: DateTime
- Description: string
- FieldName: string
- OldValue: string
- NewValue: string
- Metadata: string (JSON)
```

**Change Types** (enum):
- Created, Started, Completed, Cancelled
- Assigned, Unassigned, Reassigned
- Delegated, DelegationAccepted, DelegationDeclined, DelegationRevoked
- StatusChanged, PriorityChanged
- StepCompleted, CriterionMet
- Blocked, Unblocked
- Flagged, Unflagged
- And many more...

---

### Domain Services

#### **TaskInstanceManager**
**Responsibility**: High-level task instance operations and coordination.

**Key Operations**:
```csharp
TaskInstance CreateFromBlueprintAsync(
    taskId, projectId, workflowStatusId, ownerId, assigneeId, plannedDueDate)

TaskInstance StartAsync(taskInstanceId, startedBy)

TaskInstance CompleteAsync(
    taskInstanceId, completedBy, resolution, resolutionNote)

TaskInstance DelegateAsync(
    taskInstanceId, fromUserId, toUserId, dueBy, followupRequired)
```

---

#### **TaskInstanceProcessingService**
**Responsibility**: Coordinates complex task processing workflows.

**Key Operations**:
```csharp
TaskInstanceProcessingResult ProcessTaskCompletion(
    taskInstance, taskBlueprint, completedBy, resolution, resolutionNote)

TaskInstance CreateTaskInstanceFromBlueprint(
    taskBlueprint, projectId, workflowStatusId, ownerId, assigneeId, plannedDueDate)

void ProcessRecurringTaskOccurrence(
    taskBlueprint, projectId, workflowStatusId, occurrenceDate)
```

**Validation Logic**:
- Validates completion prerequisites
- Checks required step completion
- Enforces blocking conditions
- Applies completion policies

---

#### **TaskDependencyService**
**Responsibility**: Manages task dependency chains and validation.

**Key Operations**:
```csharp
bool CanStartTask(taskInstance, dependentInstances, dependencies)

DateTime? CalculateEarliestStartDate(
    taskInstance, dependentInstances, dependencies)

List<Guid> DetectCircularDependencies(taskId, allDependencies)
```

**Dependency Evaluation**:
- FinishToStart: predecessor must be completed
- StartToStart: predecessor must be started
- Lag time calculation
- Blocking dependency enforcement

---

#### **TaskTransitionOrchestrator**
**Responsibility**: Integrates task state changes with workflow system.

**Key Operations**:
```csharp
TaskTransitionResult TransitionTaskAsync(
    taskInstanceId, transitionId, currentUserId, userRoles, comment)

List<AvailableTransition> GetAvailableTransitionsAsync(
    taskInstanceId, currentUserId, userRoles)
```

**Orchestration Flow**:
1. Load task instance and workflow
2. Validate transition permissions
3. Evaluate transition conditions
4. Execute workflow actions
5. Update task status
6. Publish transition events

---

#### **TaskRecurrenceService**
**Responsibility**: Handles recurring task scheduling and occurrence calculation.

**Key Operations**:
```csharp
List<DateTime> CalculateNextOccurrences(
    recurrenceRule, startDate, maxOccurrences)

bool ShouldCreateOccurrence(
    taskBlueprint, checkDate, existingInstances)
```

---

### Policies

#### **ITaskStartPolicy**
```csharp
(bool CanStart, string FailureReason) Check(TaskInstance)
```

**Implementations**:
- `DefaultTaskStartPolicy`: Validates not already started, not blocked

---

#### **ITaskCompletionPolicy**
```csharp
(bool CanComplete, string FailureReason) Check(TaskInstance)
```

**Implementations**:
- `DefaultCompletionPolicy`: Validates required steps completed
- `ApprovalCompletionPolicy`: Additionally validates approval status

---

### Value Objects

#### **TaskProgress**
```csharp
- WorkflowStatusId: Guid
- TaskStatus: WorkflowStatusCategory
- ProgressPercent: double (0-100)
- IsNextAction: bool
```

**Factory Methods**:
```csharp
static TaskProgress NotStarted(workflowStatusId)
static TaskProgress InProgress(workflowStatusId, progressPercent)
static TaskProgress Completed(workflowStatusId)
```

---

#### **TaskSchedule**
```csharp
- PlannedStartDate: DateTime?
- PlannedDueDate: DateTime?
- ActualStartDate: DateTime?
- ActualDueDate: DateTime?
- CompletedAt: DateTime?
```

**Immutable Updates**:
```csharp
TaskSchedule WithPlannedDates(start, due)
TaskSchedule WithActualDates(start, due)
TaskSchedule MarkCompleted(completedAt)
```

**Derived Properties**:
```csharp
bool IsCompleted
bool IsOverdue
TimeSpan? TimeRemaining
bool HasStarted
```

---

#### **ImportanceUrgencyMatrix**
```csharp
- Importance: double (0-1)
- Urgency: double (0-1)
- Quadrant: EisenhowerQuadrant
```

**Eisenhower Quadrants**:
- Q1DoFirst (urgent + important)
- Q2Schedule (not urgent + important)
- Q3Delegate (urgent + not important)
- Q4Eliminate (not urgent + not important)

---

#### **DelegationInfo**
```csharp
- DelegatedBy: Guid
- DelegatedTo: Guid
- Status: DelegationStatus (Delegated, Accepted, Declined)
- DelegatedAt: DateTime
- RespondedAt: DateTime?
- Note: string
- ResponseNote: string
```

**Immutable State Transitions**:
```csharp
DelegationInfo Accept(responseNote)
DelegationInfo Decline(responseNote)
```

---

#### **TaskHierarchy**
```csharp
- OrderInParent: int
- ParentInstanceId: Guid?
- SubtasksCount: int
- SubtasksDoneCount: int
```

**Derived Properties**:
```csharp
bool IsParent
bool HasSubtasks
bool AllSubtasksDone
double SubtaskCompletionRate
```

---

#### **TaskDiscussion**
```csharp
- CommentsCount: int
- AttachmentsCount: int
- Watchers: IReadOnlyCollection<Guid>
- IsDiscussionLocked: bool
```

**Immutable Operations**:
```csharp
TaskDiscussion AddComment()
TaskDiscussion AddAttachment()
TaskDiscussion AddWatcher(userId)
TaskDiscussion RemoveWatcher(userId)
TaskDiscussion LockDiscussion()
```

---

#### **BlockingInfo**
```csharp
- IsBlocked: bool
- Reason: string
- BlockedAt: DateTime?
```

**Factory Methods**:
```csharp
static BlockingInfo NotBlocked
static BlockingInfo Create(reason)
```

---

#### **TaskApproval**
```csharp
- ApprovalRequestId: Guid?
- RequiresApproval: bool
- Status: ApprovalStatusEnum (NotRequired, Pending, Approved, Rejected)
- CompletedAt: DateTime?
```

**Immutable Updates**:
```csharp
static TaskApproval NoApprovalRequired()
static TaskApproval PendingApproval(approvalRequestId)
TaskApproval MarkAsApproved(approvedAt, requestId)
TaskApproval MarkAsRejected(rejectedAt, requestId)
```

---

#### **RecurrenceRule**
```csharp
- Frequency: RecurrenceFrequency (Daily, Weekly, Monthly, Yearly)
- Interval: int
- EndDate: DateTime?
- Count: int?
- DaysOfWeek: List<DayOfWeek>
```

**Behavior**:
```csharp
DateTime CalculateNextOccurrence(currentDate)
```

---

#### **TimeTrackingInfo**
```csharp
- ActualMinutes: int
- LastTrackedAt: DateTime?
```

**Immutable Update**:
```csharp
TimeTrackingInfo LogTime(minutes)
```

---

#### **ResolutionInfo**
```csharp
- Resolution: string
- ResolutionNote: string
```

**Factory**:
```csharp
static ResolutionInfo Unresolved
static ResolutionInfo Create(resolution, resolutionNote)
```

---

#### **FlagInfo**
```csharp
- IsFlagged: bool
- FlaggedReason: string
```

---

## Workflow Context

### Aggregates

#### **Workflow**
**Root Entity**: `Workflow`
**Responsibility**: Defines status-based workflow with transitions.

**Key Properties**:
```csharp
- Name: string
- Description: string
- InitialStatusId: Guid
- IsDefault: bool
- IsActive: bool
- Statuses: IReadOnlyCollection<WorkflowStatus>
- Transitions: IReadOnlyCollection<WorkflowTransition>
- ExtraProperties: ExtraPropertyDictionary
```

**Invariants**:
- Must have at least one status
- Must have exactly one initial status
- All transitions must reference valid statuses
- Cannot deactivate default workflow

**Key Behaviors**:
```csharp
void AddStatus(status)
void AddTransition(transition)
WorkflowStatus GetStatus(statusId)
List<WorkflowTransition> GetAvailableTransitions(currentStatusId)
void SetAsDefault()
void Activate()
void Deactivate()
```

---

#### **WorkflowScheme**
**Root Entity**: `WorkflowScheme`
**Responsibility**: Maps task types to workflows per project.

**Key Properties**:
```csharp
- Name: string
- ProjectId: Guid
- DefaultWorkflowId: Guid?
- IsActive: bool
- IsDraft: bool
- AllowQuickDelegation: bool
- RequireApprovalForIntegrated: bool
- Mappings: IReadOnlyCollection<WorkflowSchemeMapping>
```

**State Machine**:
```
Draft → Published → [Active | Inactive]
```

**Key Behaviors**:
```csharp
void AddMapping(mapping)
void RemoveMapping(taskTypeId)
Guid GetWorkflowForTaskType(taskTypeId)
void SetDefaultWorkflow(workflowId)
void Publish()
void Activate()
void Deactivate()
void SetDelegationMode(allowQuick, requireApprovalForIntegrated)
```

**Invariants**:
- Cannot activate draft scheme
- Must have default workflow OR at least one mapping
- Cannot publish empty scheme

---

#### **TransitionPermissionPolicy**
**Root Entity**: `TransitionPermissionPolicy`
**Responsibility**: Controls who can execute workflow transitions.

**Key Properties**:
```csharp
- Scope: PermissionPolicyScope (Global, Project, Custom)
- ProjectId: Guid?
- WorkflowId: Guid
- TransitionId: Guid
- Name: string
- IsActive: bool
- Priority: int
- AllowedRoles: IReadOnlyCollection<string>
- AllowedUserIds: IReadOnlyCollection<Guid>
```

**Key Behaviors**:
```csharp
bool CanUserPerform(userId, userRoles)
void AddAllowedRole(role)
void AddAllowedUser(userId)
void RemoveAllowedRole(role)
void RemoveAllowedUser(userId)
void Activate()
void Deactivate()
void SetPriority(priority)
```

**Permission Evaluation**:
- No restrictions = allow all
- OR logic: user ID match OR any role match
- Scope: Project-specific > Global
- Priority: Higher priority evaluated first

---

### Entities

#### **WorkflowStatus**
**Parent**: Workflow
**Responsibility**: Represents a state in the workflow.

**Key Properties**:
```csharp
- WorkflowId: Guid
- Name: string
- Description: string
- Category: WorkflowStatusCategory (Todo, InProgress, Done, Cancelled, Blocked)
- Color: string (hex)
- OrderIndex: int
- IsInitial: bool
- IsResolution: bool
- ExtraProperties: ExtraPropertyDictionary
```

**Derived Properties**:
```csharp
bool IsTodo
bool IsInProgress
bool IsDone
bool IsCancelled
bool IsBlocked
bool IsTerminal  // Done or Cancelled
```

**Default Colors by Category**:
- Todo: #94a3b8 (slate)
- InProgress: #3b82f6 (blue)
- Done: #22c55e (green)
- Cancelled: #ef4444 (red)
- Blocked: #FFBF00 (amber)

---

#### **WorkflowTransition**
**Parent**: Workflow
**Responsibility**: Defines allowed status changes with conditions and actions.

**Key Properties**:
```csharp
- WorkflowId: Guid
- Name: string
- FromStatusId: Guid
- ToStatusId: Guid
- Purpose: TransitionPurpose (Standard, Approval, Rejection, Cancellation)
- RequiresComment: bool
- RequiresResolution: bool
- IsGlobal: bool  // Can transition from any status
- OrderIndex: int
- ButtonLabel: string
- ButtonColor: string
- Actions: IReadOnlyCollection<WorkflowAction>
- Conditions: IReadOnlyCollection<WorkflowCondition>
- Validators: IReadOnlyDictionary<string, object>
```

**Invariants**:
- FromStatusId cannot equal ToStatusId
- Both statuses must exist in workflow

**Key Behaviors**:
```csharp
void AddAction(action)
void SetCondition(condition)
void SetValidator(key, value)
void SetPurpose(purpose)
```

**Derived Properties**:
```csharp
bool HasConditions
bool HasValidators
```

---

#### **WorkflowAction**
**Parent**: WorkflowTransition
**Responsibility**: Automated action execution on transition.

**Key Properties**:
```csharp
- TransitionId: Guid
- Name: string
- ActionType: WorkflowActionType
- Configuration: Dictionary<string, object>
- OrderIndex: int
- IsEnabled: bool
- IsAsync: bool
- ContinueOnError: bool
- Conditions: IReadOnlyDictionary<string, object>
```

**Action Types** (enum):
```csharp
// Assignment Actions
AssignToUser, AssignToReporter, AssignToCurrentUser, ClearAssignee

// Field Actions
UpdateField, SetCustomField, ValidateFields, UpdatePriority, SetDueDate

// Communication Actions
SendEmail, SendNotification, AddComment

// Task Actions
CreateSubtask, LinkTask, DelegateTask

// Approval Actions
InitiateApproval, CompleteApproval

// Integration Actions
TriggerWebhook, ExecuteScript, FireDomainEvent
```

**Key Behaviors**:
```csharp
void UpdateConfiguration(newConfiguration)
void SetCondition(key, value)
```

---

#### **WorkflowSchemeMapping**
**Parent**: WorkflowScheme
**Responsibility**: Links task type to specific workflow.

**Key Properties**:
```csharp
- WorkflowSchemeId: Guid
- TaskTypeId: Guid
- WorkflowId: Guid
- IsActive: bool
```

---

#### **WorkflowCustomField**
**Parent**: WorkflowScheme
**Responsibility**: Configures custom fields per workflow context.

**Key Properties**:
```csharp
- WorkflowSchemeId: Guid
- CustomFieldDefinitionId: Guid
- IsRequired: bool
- DefaultValue: string
- DisplayOrder: int
- VisibleInStatusIds: IReadOnlyCollection<Guid>
- RequiredInStatusIds: IReadOnlyCollection<Guid>
- ReadOnlyInStatusIds: IReadOnlyCollection<Guid>
- RequiredForTransitionIds: IReadOnlyCollection<Guid>
```

**Key Behaviors**:
```csharp
bool IsVisibleInStatus(statusId)
bool IsRequiredInStatus(statusId)
bool IsReadOnlyInStatus(statusId)
bool IsRequiredForTransition(transitionId)

void AddVisibleInStatus(statusId)
void MakeRequiredInStatus(statusId)
void MakeReadOnlyInStatus(statusId)
void RequireForTransition(transitionId)
```

---

### Domain Services

#### **WorkflowTransitionService**
**Responsibility**: Orchestrates workflow transition execution.

**Key Operations**:
```csharp
TransitionExecutionResult ExecuteTransitionAsync(
    taskInstanceId, workflow, transition, currentUserId, userRoles, projectId, customFields, comment)

TransitionValidationResult ValidateTransitionAsync(
    workflow, transition, currentUserId, userRoles, projectId, customFields, comment)

List<WorkflowAction> GetOrderedActions(transition)

WorkflowTransitionExecutedEto CreateTransitionEvent(...)
```

**Validation Steps**:
1. Check user permissions (via TransitionPermissionPolicy)
2. Validate comment requirement
3. Validate target status exists
4. Evaluate all transition conditions
5. Execute ordered actions
6. Publish transition event

---

#### **WorkflowActionExecutorService**
**Responsibility**: Executes workflow actions using Strategy pattern.

**Key Operations**:
```csharp
ActionExecutionResult ExecuteActionsAsync(
    actions, context)
```

**Execution Flow**:
1. Filter enabled actions
2. For each action:
   - If async: queue for background execution
   - If sync: find executor and execute
3. Handle ContinueOnError flag
4. Return execution results

**Action Context**:
```csharp
class WorkflowActionContext {
    TaskInstanceId, WorkflowId, TransitionId, 
    ExecutedBy, Comment, CustomData
}
```

---

#### **IWorkflowActionExecutor** (Strategy Interface)
**Implementations**:
- `AssignUserActionExecutor`
- `UpdateFieldActionExecutor`
- `UpdatePriorityActionExecutor`
- `SetDueDateActionExecutor`
- `ValidateFieldsActionExecutor`
- `SendEmailActionExecutor`
- `SendNotificationActionExecutor`
- `AddCommentActionExecutor`
- `CreateSubtaskActionExecutor`
- `LinkTaskActionExecutor`
- `DelegateTaskActionExecutor`
- `InitiateApprovalActionExecutor`
- `CompleteApprovalActionExecutor`
- `TriggerWebhookActionExecutor`
- `ExecuteScriptActionExecutor`
- `FireDomainEventActionExecutor`

**Interface**:
```csharp
bool CanExecute(WorkflowAction action)
Task<ActionExecutionResult> ExecuteAsync(action, context)
```

---

#### **WorkflowConditionEvaluationService**
**Responsibility**: Evaluates workflow transition conditions.

**Key Operations**:
```csharp
ConditionEvaluationAggregateResult EvaluateAllAsync(
    conditions, context)  // AND logic

ConditionEvaluationAggregateResult EvaluateAnyAsync(
    conditions, context)  // OR logic

ConditionEvaluationResult EvaluateSingleAsync(
    condition, context)
```

**Condition Context**:
```csharp
class WorkflowConditionContext {
    TaskInstanceId, CurrentUserId, UserRoles,
    CustomFieldValues, AdditionalData
}
```

---

#### **IWorkflowConditionEvaluator** (Strategy Interface)
**Implementations**:
- `FieldValueConditionEvaluator`
- `RoleBasedConditionEvaluator`

**Field Value Operators**:
- equals, notEquals
- greaterThan, lessThan, greaterThanOrEquals, lessThanOrEquals
- contains, notContains
- isEmpty, isNotEmpty

**Role Operators**:
- hasRole, hasAnyRole, hasAllRoles, doesNotHaveRole

---

#### **WorkflowValidationService**
**Responsibility**: Validates workflow configuration integrity.

**Key Operations**:
```csharp
WorkflowValidationResult ValidateWorkflow(workflow)
SchemeValidationResult ValidateWorkflowScheme(scheme, workflows)
```

**Validation Rules**:
- Must have at least one status
- Exactly one initial status
- At least one terminal status
- All statuses reachable from initial
- Valid transition endpoints
- Required action configurations

---

### Value Objects

#### **WorkflowCondition**
```csharp
- Key: string  // Field name
- Value: object  // Expected value
- Operator: string  // Comparison operator
```

**Examples**:
```csharp
new WorkflowCondition("Priority", "High", "equals")
new WorkflowCondition("EstimatedHours", 40, "greaterThan")
new WorkflowCondition("Assignee", null, "isEmpty")
```

---

## Domain Events

### Approval Events

#### **ApprovalRequestCreatedEto**
```csharp
RequestId, WorkflowId, EntityType, EntityId, 
RequestedBy, TenantId, CreatedAt
```

#### **ApprovalRequestProcessedEto**
```csharp
RequestId, TenantId, Status, ProcessedAt, 
ProcessedBy, EntityType, EntityId
```

#### **ApprovalWorkflowCompletedEto**
```csharp
RequestId, WorkflowId, TenantId, IsApproved, 
CompletedAt, TotalSteps, CompletedSteps, 
EntityType, EntityId
```

---

### Project Events

#### **ProjectCreatedEto**
```csharp
ProjectId, OwnerId, Title, TenantId, OccurredAt
```

#### **ProjectStatusChangedEto**
```csharp
ProjectId, TenantId, OldStatus, NewStatus, 
ChangedAt, ChangedBy
```

#### **ProjectCompletedEto**
```csharp
ProjectId, TenantId, CompletedAt, FinalProgress, 
CompletedBy
```

#### **SprintCompletedEto**
```csharp
SprintId, ProjectId, TenantId, CompletedAt, 
PlannedStoryPoints, CompletedStoryPoints, 
CompletionRate
```

---

### Task Events

#### **TaskCreatedEto**
```csharp
TaskId, TenantId, Title, OwnerId, CreatedAt
```

#### **TaskInstanceCreatedEto**
```csharp
TaskInstanceId, TaskId, TenantId, OwnerId, CreatedAt
```

#### **TaskStartedEto**
```csharp
TaskInstanceId, TenantId, StartedBy, StartedAt
```

#### **TaskCompletedEto**
```csharp
TaskInstanceId, TenantId, CompletedBy, CompletedAt, 
ActualMinutes, Resolution
```

#### **TaskAssignedEto**
```csharp
TaskInstanceId, TenantId, AssignedTo, AssignedBy, 
AssignedAt
```

#### **TaskUnassignedEto**
```csharp
TaskInstanceId, TenantId
```

#### **TaskDelegatedEto**
```csharp
TaskInstanceId, TenantId, DelegatedBy, DelegatedTo, 
DelegatedAt, Note
```

#### **TaskDelegationAcceptedEto**
```csharp
TaskInstanceId, TenantId, AcceptedBy, AcceptedAt, 
ResponseNote
```

#### **TaskDelegationDeclinedEto**
```csharp
TaskInstanceId, TenantId, DeclinedBy, DeclinedAt, 
Reason
```

#### **TaskBlockedEto**
```csharp
TaskInstanceId, TenantId, Reason, BlockedAt
```

---

### Workflow Events

#### **WorkflowTransitionExecutedEto**
```csharp
TaskId, WorkflowId, TransitionId, FromStatusId, 
ToStatusId, ExecutedBy, TenantId, ExecutedAt, Comment
```

#### **WorkflowTriggeredDomainEvent** (Generic)
```csharp
EventType, TaskInstanceId, WorkflowId, ExecutedBy, 
TenantId, EventData, TriggeredAt
```

---

## Cross-Context Integration

### Task ↔ Approval Integration

**Flow**: Task requires approval
1. Workflow action: `InitiateApproval`
2. Creates `ApprovalRequest` linked to `TaskInstance`
3. Updates `TaskInstance.Approval` value object
4. On approval completion:
   - Event: `ApprovalWorkflowCompletedEto`
   - Handler: `ApprovalCompletedEventHandler`
   - Updates `TaskInstance.Approval.Status`

**Key Integration Points**:
```csharp
// TaskInstance Value Object
TaskApproval {
    ApprovalRequestId: Guid?
    RequiresApproval: bool
    Status: ApprovalStatusEnum
}

// Workflow Action
InitiateApprovalActionExecutor:
    - Creates ApprovalRequest
    - Links to TaskInstance via EntityId
    - Sets ApprovalRequestId on TaskInstance

// Event Handler
ApprovalCompletedEventHandler:
    - Listens for ApprovalWorkflowCompletedEto
    - Filters by EntityType == "TaskInstance"
    - Updates TaskInstance.Approval status
```

---

### Task ↔ Workflow Integration

**Flow**: Task status transitions
1. User initiates transition via `TaskTransitionOrchestrator`
2. Service validates permissions via `TransitionPermissionPolicy`
3. Evaluates transition conditions
4. Executes workflow actions (via `WorkflowActionExecutorService`)
5. Updates task status
6. Publishes `WorkflowTransitionExecutedEto`

**Key Integration Service**:
```csharp
TaskTransitionOrchestrator {
    - GetAvailableTransitionsAsync()
      → Checks permissions
      → Validates conditions
      → Returns available transitions
    
    - TransitionTaskAsync()
      → Executes full transition flow
      → Coordinates workflow + task updates
}
```

---

### Project ↔ Task Integration

**Relationships**:
```csharp
Task.ProjectId → Project
TaskInstance.ProjectId → Project (inherited from Task)

Project {
    - HasTasks: via Task.ProjectId query
    - WorkflowSchemeId: determines task workflows
}
```

**Workflow Scheme Resolution**:
```
Project.WorkflowSchemeId
  → WorkflowScheme.GetWorkflowForTaskType(TaskInstance.TaskTypeId)
  → Workflow with statuses and transitions
```

---

### Project ↔ Approval Integration

**Dependency Approval Flow**:
```csharp
ProjectDependency {
    ApprovalRequestId: Guid?
}

// When dependency requires approval:
1. Create ApprovalRequest for ProjectDependency
2. Link via ApprovalRequestId
3. Block project progression until approved
```

---

## Repository Interfaces

### Approval Repositories

#### **IApprovalWorkflowRepository**
```csharp
Task<ApprovalWorkflow> GetWithStepsAsync(id)
Task<List<ApprovalWorkflow>> GetActiveWorkflowsAsync()
Task<List<ApprovalWorkflow>> GetByTypeAsync(workflowType)
```

#### **IApprovalRequestRepository**
```csharp
Task<ApprovalRequest> GetWithDecisionsAsync(id)
Task<List<ApprovalRequest>> GetPendingRequestsAsync()
Task<List<ApprovalRequest>> GetRequestsByEntityAsync(entityType, entityId)
Task<List<ApprovalRequest>> GetListAsync(spec)
```

---

### Project Repositories

#### **IProjectRepository**
```csharp
Task<Project> GetWithDetailsAsync(id)
Task<List<Project>> GetListAsync(specification, includeDetails)
Task<List<Project>> GetByOwnerAsync(ownerId)
Task<List<Project>> GetOverdueProjectsAsync()
```

#### **IProjectDependencyRepository**
```csharp
Task<List<ProjectDependency>> GetByProjectIdAsync(projectId)
Task<List<ProjectDependency>> GetBlockingDependenciesAsync(projectId)
Task<List<ProjectDependency>> GetOverdueDependenciesAsync()
```

#### **IProjectSprintRepository**
```csharp
Task<List<ProjectSprint>> GetByProjectIdAsync(projectId)
Task<ProjectSprint> GetActiveSprintAsync(projectId)
```

#### **IProjectSuccessCriterionRepository**
```csharp
Task<List<ProjectSuccessCriterion>> GetByProjectIdAsync(projectId)
Task<List<ProjectSuccessCriterion>> GetUnmetCriteriaAsync(projectId)
```

---

### Task Repositories

#### **ITaskRepository**
```csharp
Task<Task> GetWithDetailsAsync(id)
Task<List<Task>> GetListAsync(specification)
Task<List<Task>> GetRecurringTasksDueForProcessingAsync(currentDate)
```

#### **ITaskInstanceRepository**
```csharp
Task<TaskInstance> GetWithDetailsAsync(id)
Task<List<TaskInstance>> GetListAsync(spec, includeDetails)
Task<List<TaskInstance>> GetOverdueTasksAsync(asOfDate)
Task<List<TaskInstance>> GetTaskInstancesByProjectAsync(projectId)
Task<Dictionary<Guid, int>> GetTaskInstanceCountByStatusAsync(projectId)
```

#### **ITaskTypeRepository**
```csharp
Task<TaskType> GetWithCustomFieldsAsync(id)
Task<List<TaskType>> GetActiveTaskTypesAsync()
Task<TaskType> FindByNameAsync(name)
```

---

### Workflow Repositories

#### **IWorkflowRepository**
```csharp
Task<Workflow> GetWithDetailsAsync(id)
Task<Workflow> GetDefaultWorkflowAsync()
```

#### **IWorkflowSchemeRepository**
```csharp
Task<WorkflowScheme> GetWithMappingsAsync(id)
Task<WorkflowScheme> GetActiveSchemeForProjectAsync(projectId)
```

#### **ITransitionPermissionPolicyRepository**
```csharp
Task<TransitionPermissionPolicy> GetApplicablePolicyAsync(
    transitionId, projectId)

Task<List<TransitionPermissionPolicy>> GetPoliciesForTransitionAsync(
    transitionId, projectId)

Task<List<TransitionPermissionPolicy>> GetPoliciesByWorkflowAsync(
    workflowId, includeInactive)

Task<bool> PolicyExistsAsync(
    transitionId, projectId, scope)

Task<List<TransitionPermissionPolicy>> GetPoliciesByUserAsync(userId)
Task<List<TransitionPermissionPolicy>> GetPoliciesByRoleAsync(role)
```

---

## Specifications

### Approval Specifications
- `PendingApprovalRequestsSpecification`
- `OverdueApprovalRequestsSpecification`
- `ApprovalRequestsByWorkflowSpecification`
- `ApprovalRequestsByEntitySpecification`
- `UserPendingApprovalsSpecification`
- `ActiveWorkflowsSpecification`
- `WorkflowsWithTimeoutSpecification`
- `WorkflowsByStrategySpecification`

### Project Specifications
- `ActiveProjectsSpecification`
- `OverdueProjectsSpecification`
- `ProjectsByOwnerSpecification`
- `HealthyProjectsSpecification`
- `ProjectsInQuadrantSpecification`
- `ProjectsDueInNextDaysSpecification`
- `ActiveSprintsSpecification`
- `CompletedSprintsSpecification`
- `ActiveDependenciesSpecification`
- `BlockingDependenciesSpecification`
- `OverdueDependenciesSpecification`
- `PendingRequirementsSpecification`

### Task Specifications
- `ActiveTasksSpecification`
- `RecurringTasksSpecification`
- `TasksByOwnerSpecification`
- `TasksByProjectSpecification`
- `ActiveTaskInstancesSpecification`
- `OverdueTaskInstancesSpecification`
- `TaskInstancesByAssigneeSpecification`
- `HighPriorityTaskInstancesSpecification`
- `BlockedTaskInstancesSpecification`
- `NextActionTasksSpecification`
- `DelegatedTasksSpecification`
- `CompletedInDateRangeSpecification`
- `ActiveTaskTypesSpecification`

### Workflow Specifications
- `ActiveWorkflowsSpecification`
- `DefaultWorkflowSpecification`

---

## Key Business Rules Summary

### Approval Rules
1. **Step Progression**: Must complete current step before moving to next
2. **Rejection Handling**: Any rejection terminates entire workflow
3. **Approval Requirements**:
   - Any: 1+ approval
   - All: All approvers must approve
   - Majority: >50% approval
   - Custom: Specific count required
4. **Self-Approval**: Configurable per step
5. **Timeout Actions**: Auto-approve, escalate, or custom
6. **Delegation**: Approval can be delegated if allowed

### Project Rules
1. **Status Transitions**: 
   - Can only start Planned or OnHold projects
   - Cannot cancel Completed projects
   - Cannot archive Active projects
2. **Priority**: Eisenhower quadrant auto-calculated from importance/urgency
3. **Success Criteria**: Max 20 per project
4. **Dependencies**:
   - No self-dependencies
   - Circular dependencies detected and prevented
   - Lag time supported
5. **Sprint Duration**: 7-30 days only
6. **Health Calculation**: Based on progress vs. time, success criteria, and due date

### Task Rules
1. **Blueprint Limits**:
   - Max 50 action steps
   - Max 20 acceptance criteria
   - Max 50 dependencies
2. **Instance Lifecycle**:
   - Cannot complete blocked tasks
   - Cannot complete without required steps
   - Cannot delegate to self
   - Cannot start already-started task
3. **Delegation Flow**:
   - Delegated → [Accepted | Declined]
   - Can be revoked before acceptance
4. **Dependency Types**:
   - FinishToStart: Most common
   - StartToStart: Parallel tasks
   - FinishToFinish: Synchronized completion
   - StartToFinish: Rare inverse dependency
5. **Recurrence**: Supports daily, weekly, monthly, yearly with intervals
6. **History Tracking**: All changes logged with old/new values

### Workflow Rules
1. **Status Configuration**:
   - Exactly one initial status
   - At least one terminal status
   - All statuses must be reachable
2. **Transition Validation**:
   - FromStatus ≠ ToStatus
   - Both statuses must exist
   - Conditions evaluated with AND logic
3. **Permission Evaluation**:
   - Project-specific policies override global
   - Higher priority evaluated first
   - Empty restrictions = allow all
   - OR logic: user ID OR role match
4. **Action Execution**:
   - Ordered by OrderIndex
   - Async actions queued separately
   - ContinueOnError flag respected
5. **Scheme Rules**:
   - Cannot activate draft
   - Must have default workflow OR mappings
   - Cannot publish empty scheme

---

## Constants

### Approval Constants
```csharp
MaxSuccessCriteria = 20
DefaultImportance = 0.5
DefaultUrgency = 0.5
```

### Project Constants
```csharp
MinSprintDurationDays = 7
MaxSprintDurationDays = 30
DefaultSprintVelocity = 20
MaxSuccessCriteria = 20
```

### Task Constants
```csharp
MaxTitleLength = 500
MaxDescriptionLength = 4000
MaxActionSteps = 50
MaxAcceptanceCriteria = 20
MaxSubtasks = 100
MaxWatchers = 50
MaxLabels = 20
MaxDependencies = 50
MinImportance = 0.0
MaxImportance = 1.0
MinUrgency = 0.0
MaxUrgency = 1.0
```

### Workflow Constants
```csharp
MaxTitleLength = 200
MaxDescriptionLength = 5000
MaxWorkflowStatusesPerWorkflow = 50
MaxTransitionsPerWorkflow = 100
MaxActionsPerTransition = 20
MaxAllowedRolesPerTransition = 10
MaxAllowedUsersPerTransition = 50
```

---

## Design Patterns Applied

1. **Aggregate Pattern**: Clear boundaries with root entities
2. **Domain Events**: Decoupled cross-context communication
3. **Value Objects**: Immutable, self-validating data structures
4. **Specification Pattern**: Reusable query logic
5. **Strategy Pattern**: Pluggable action executors and condition evaluators
6. **Facade Pattern**: TaskTransitionOrchestrator simplifies complex operations
7. **Policy Pattern**: Pluggable start/completion policies
8. **State Pattern**: Clear state machines with validation
9. **Factory Pattern**: Value object creation methods
10. **Repository Pattern**: Clean data access abstraction

---

## Conclusion

This domain model provides a comprehensive, well-structured foundation for a work management system with:

- **Clear separation of concerns** across 4 bounded contexts
- **Rich domain models** with behavior and invariants
- **Flexible workflow engine** with conditions and actions
- **Comprehensive approval system** with configurable steps
- **Project tracking** with health metrics and dependencies
- **Full task lifecycle** from blueprint to completion
- **Event-driven architecture** for cross-context integration
- **Extensibility** through policies, strategies, and specifications

The design follows DDD principles, SOLID principles, and ABP Framework best practices throughout.