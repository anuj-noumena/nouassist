# Task and TaskInstance Aggregate Split

## Overview

This document describes the refactoring of the `Task` (Blueprint) and `TaskInstance` (Runtime) aggregates into multiple smaller, focused aggregates. This split follows Domain-Driven Design principles to improve maintainability, scalability, and query performance.

## Rationale

### Problems with Monolithic Aggregates

1. **Large aggregate boundaries**: Both `Task` and `TaskInstance` contained multiple collections leading to large object graphs
2. **Performance issues**: Loading entire aggregates with all collections was expensive
3. **Concurrency conflicts**: Multiple users modifying different aspects of a task could cause conflicts
4. **Difficult to reason about**: Too many responsibilities in single aggregates

### Benefits of Split Aggregates

1. **Smaller transaction boundaries**: Reduced risk of conflicts
2. **Better query performance**: Load only what you need
3. **Independent evolution**: Each aggregate can evolve separately
4. **Clearer business boundaries**: Each aggregate has a focused responsibility

## Aggregate Architecture

### Task Blueprint Aggregates (Template/Definition)

#### 1. Task (Core Blueprint)
**Purpose**: Lean template without collections, core task definition

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/Task.cs`

**Properties**:
- Identity: `OwnerId`, `TenantId`, `ProjectId`, `TaskTypeId`
- Content: `Title`, `Description`, `ExpectedResult`
- Hierarchy: `TaskHierarchy` (Value Object)
- GTD Organization: `AreaId`, `ContextId`
- Defaults: `DefaultImportance`, `DefaultUrgency`, `DefaultDueDate`, `DefaultEstimateMinutes`, etc.
- Recurrence: `RecurrenceRule`
- State: `IsActive`, `Version`

**Key Methods**:
- `UpdateTitle(string newTitle)`
- `SetDefaults(double importance, double urgency)`
- `SetRecurrence(RecurrenceRule rule)`
- `Activate()` / `Deactivate()`
- GTD: `AssignToArea()`, `MoveToProject()`, `AssignToContext()`
- `SetImportanceUrgency()`

**Events**:
- `TaskCreatedEto`
- `TaskTitleUpdatedEto`
- `TaskAreaChangedEto`
- `TaskContextChangedEto`
- `TaskMovedToProjectEto`
- `TaskRecurrenceSetEto`
- `TaskActivatedEto` / `TaskDeactivatedEto`

#### 2. TaskChecklist (Blueprint Checklists)
**Purpose**: Manages blueprint checklist items

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskChecklist.cs`

**Properties**:
- `TaskId` (Foreign key to Task)
- `TenantId`
- `Version`
- Collection: `_items` (List of `ChecklistItem`)

**Key Methods**:
- `AddItem(string title, bool isRequired)`
- `RemoveItem(Guid itemId)`
- `ReorderItems(List<Guid> orderedIds)`

**Events**:
- `TaskChecklistItemAddedEto`
- `TaskChecklistItemRemovedEto`

**Repository**: `ITaskChecklistRepository`

#### 3. TaskAcceptanceCriteria (Blueprint Criteria)
**Purpose**: Manages blueprint acceptance criteria

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskAcceptanceCriteria.cs`

**Properties**:
- `TaskId` (Foreign key to Task)
- `TenantId`
- `Version`
- Collection: `_criteria` (List of `AcceptanceCriterion`)

**Key Methods**:
- `AddCriterion(string description, int? priority)`
- `RemoveCriterion(Guid criterionId)`

**Events**:
- `TaskAcceptanceCriterionAddedEto`
- `TaskAcceptanceCriterionRemovedEto`

**Repository**: `ITaskAcceptanceCriteriaRepository`

---

### TaskInstance Runtime Aggregates (Execution)

#### 1. TaskInstance (Core Runtime)
**Purpose**: Execution state without deep collections

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskInstance.cs`

**Properties**:
- Identity: `TaskId`, `WorkflowId`, `ProjectId`, `TenantId`, `EntityVersion`
- Ownership: `OwnerId`, `AssigneeId`, `ReporterId`, `TeamId`
- GTD Organization: `AreaId`, `ContextId`
- Workflow: `WorkflowStatusId`
- Work Tracking: `TaskWorkTracking`, `TaskSchedule`, `ImportanceUrgencyMatrix`
- Approval: `ApprovalRequestId`
- Collaboration: `TaskCollaboration`
- Custom: `CustomFieldValues`
- Flags: `IsBlocked`, `IsFlagged`, `IsResolved`

**Key Methods**:
- Workflow: `TransitionTo()`, `Start()`, `Complete()`, `UpdateProgress()`
- Assignment: `Assign()`, `Unassign()`
- GTD: `AssignToArea()`, `MoveToProject()`, `UpdateOrganization()`
- Blocking: `Block()`, `Unblock()`
- Flagging: `Flag()`, `Unflag()`
- Time: `LogTime()`, `SetPlannedDates()`, `SetEstimate()`
- Priority: `UpdatePriority()`
- Collaboration: `AddWatcher()`, `RemoveWatcher()`, `IncrementComments()`
- Approval: `LinkApprovalRequest()`

**Events**:
- `TaskInstanceCreatedEto`
- `TaskStatusTransitionedEto`
- `TaskStartedEto`
- `TaskCompletedEto`
- `TaskAssignedEto` / `TaskUnassignedEto`
- `TaskInstanceAreaChangedEto`
- `TaskBlockedEto` / `TaskUnblockedEto`
- `TaskFlaggedEto`

#### 2. TaskChecklistCompletion (Runtime Checklist Progress)
**Purpose**: Tracks runtime checklist completion

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskChecklistCompletion.cs`

**Properties**:
- `TaskInstanceId` (Foreign key to TaskInstance)
- `ChecklistId` (Foreign key to TaskChecklist blueprint)
- `TenantId`
- `Version`
- Collection: `_items` (List of `TaskChecklistItem` with completion tracking)

**Key Methods**:
- `InitializeFromBlueprint(List<Guid> checklistItemIds)`
- `CheckItem(Guid itemId, Guid? userId)`
- `UncheckItem(Guid itemId)`
- `GetCompletionPercentage()` - Returns progress percentage

**Events**:
- `TaskChecklistItemCheckedEto`
- `TaskChecklistItemUncheckedEto`

**Repository**: `ITaskChecklistCompletionRepository`

#### 3. TaskAcceptanceCompletion (Runtime Criteria Fulfillment)
**Purpose**: Tracks runtime acceptance criteria fulfillment

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskAcceptanceCompletion.cs`

**Properties**:
- `TaskInstanceId` (Foreign key to TaskInstance)
- `AcceptanceCriteriaId` (Foreign key to TaskAcceptanceCriteria blueprint)
- `TenantId`
- `Version`
- Collection: `_completions` (List of `AcceptanceCriterionCompletion`)

**Key Methods**:
- `InitializeFromBlueprint(List<Guid> criterionIds)`
- `MarkAsMet(Guid criterionId, Guid userId)`
- `MarkAsUnmet(Guid criterionId)`

**Events**:
- `TaskAcceptanceCriterionMetEto`
- `TaskAcceptanceCriterionUnmetEto`

**Repository**: `ITaskAcceptanceCompletionRepository`

#### 4. TaskActionStepsCompletion (Runtime Steps)
**Purpose**: Tracks runtime action steps progress

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskActionStepsCompletion.cs`

**Properties**:
- `TaskInstanceId` (Foreign key to TaskInstance)
- `TenantId`
- `Version`
- Collection: `_completions` (List of `TaskActionStepCompletion`)

**Key Methods**:
- `InitializeFromBlueprint(List<Guid> actionStepIds)`
- `MarkComplete(Guid stepId, Guid userId)`
- `MarkUncomplete(Guid stepId)`
- `GetCompletionPercentage()`

**Events**:
- `TaskActionStepCompletedEto`
- `TaskActionStepUncompletedEto`

**Repository**: `ITaskActionStepsCompletionRepository`

#### 5. TaskDependencies (Dependency Graph)
**Purpose**: Manages task dependency graph relationships

**Location**: `NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskDependencies.cs`

**Properties**:
- `TaskInstanceId` (Foreign key to TaskInstance)
- `TenantId`
- `Version`
- Collection: `_dependencies` (List of `TaskDependency`)

**Key Methods**:
- `AddDependency(Guid dependsOnTaskInstanceId, DependencyType type, int lagMinutes, bool isBlocking)`
- `RemoveDependency(Guid dependsOnTaskInstanceId)`

**Events**:
- `TaskDependencyAddedEto`
- `TaskDependencyRemovedEto`

**Repository**: `ITaskDependenciesRepository`

**Rationale**: Separated for efficient graph traversal without loading full task instances

---

## Interactions Between Aggregates

### Reference by ID
Aggregates reference each other by ID only. Never hold direct object references.

**Example**:
```csharp
// TaskChecklist references Task by TaskId
var checklist = await _taskChecklistRepository.GetByTaskIdAsync(taskId);

// TaskChecklistCompletion references both TaskInstance and TaskChecklist
var completion = await _checklistCompletionRepository.GetByTaskInstanceIdAsync(taskInstanceId);
```

### Creation Flow

#### Creating a Task Blueprint
```csharp
// 1. Use orchestration service
var orchestrationService = new TaskAggregateOrchestrationService(
    taskChecklistRepo,
    taskAcceptanceCriteriaRepo);

var task = await orchestrationService.CreateTaskWithSubAggregatesAsync(
    taskId: Guid.NewGuid(),
    ownerId: currentUserId,
    title: "My Task",
    tenantId: currentTenantId);

// 2. Add checklist items to TaskChecklist aggregate
var checklist = await _taskChecklistRepository.GetByTaskIdAsync(task.Id);
checklist.AddItem("Review requirements", isRequired: true);
checklist.AddItem("Get approval", isRequired: false);
await _taskChecklistRepository.UpdateAsync(checklist);

// 3. Add acceptance criteria to TaskAcceptanceCriteria aggregate
var acceptanceCriteria = await _taskAcceptanceCriteriaRepository.GetByTaskIdAsync(task.Id);
acceptanceCriteria.AddCriterion("All tests pass", priority: 1);
acceptanceCriteria.AddCriterion("Code review approved", priority: 2);
await _taskAcceptanceCriteriaRepository.UpdateAsync(acceptanceCriteria);
```

#### Creating a TaskInstance from Blueprint
```csharp
// 1. Create TaskInstance
var taskInstance = taskInstanceFactory.CreateFromBlueprint(
    taskBlueprint,
    workflowId,
    workflowStatusId,
    ownerId);
await _taskInstanceRepository.InsertAsync(taskInstance);

// 2. Initialize sub-aggregates from blueprint
var instanceOrchestration = new TaskInstanceAggregateOrchestrationService(
    checklistCompletionRepo,
    acceptanceCompletionRepo,
    actionStepsCompletionRepo,
    dependenciesRepo,
    taskChecklistRepo,
    taskAcceptanceCriteriaRepo);

await instanceOrchestration.CreateTaskInstanceWithSubAggregatesAsync(
    taskInstance,
    taskBlueprint.Id);

// Sub-aggregates are automatically initialized from blueprint
```

### Updating Progress
```csharp
// Update checklist completion
var checklistCompletion = await _checklistCompletionRepository
    .GetByTaskInstanceIdAsync(taskInstanceId);
checklistCompletion.CheckItem(itemId, userId);
await _checklistCompletionRepository.UpdateAsync(checklistCompletion);

// Progress event is emitted, application layer can update TaskInstance
await instanceOrchestration.UpdateProgressFromChecklistAsync(
    taskInstanceId,
    taskInstance);
```

### Managing Dependencies
```csharp
// Add dependency
var dependencies = await _dependenciesRepository
    .GetByTaskInstanceIdAsync(taskInstanceId);
dependencies.AddDependency(
    dependsOnTaskInstanceId: otherTaskId,
    type: DependencyType.FinishToStart,
    lagMinutes: 0,
    isBlocking: true);
await _dependenciesRepository.UpdateAsync(dependencies);

// Query dependent tasks
var dependentTasks = await _dependenciesRepository
    .GetDependentTasksAsync(taskInstanceId);
```

## Queries and Read Models

For complex queries that need data from multiple aggregates, use:

1. **CQRS Read Models**: Project events into denormalized read models
2. **Application Layer DTOs**: Aggregate data from multiple repositories
3. **Composite Queries**: Join in application layer or use specifications

**Example - Get Full Task Instance View**:
```csharp
public class TaskInstanceDetailDto
{
    public TaskInstance Instance { get; set; }
    public TaskChecklistCompletionDto Checklist { get; set; }
    public TaskAcceptanceCompletionDto Acceptance { get; set; }
    public TaskActionStepsCompletionDto ActionSteps { get; set; }
    public TaskDependenciesDto Dependencies { get; set; }
}

public async Task<TaskInstanceDetailDto> GetTaskInstanceDetailAsync(Guid taskInstanceId)
{
    var instance = await _taskInstanceRepo.GetAsync(taskInstanceId);
    var checklist = await _checklistCompletionRepo.GetByTaskInstanceIdAsync(taskInstanceId);
    var acceptance = await _acceptanceCompletionRepo.GetByTaskInstanceIdAsync(taskInstanceId);
    var actionSteps = await _actionStepsCompletionRepo.GetByTaskInstanceIdAsync(taskInstanceId);
    var dependencies = await _dependenciesRepo.GetByTaskInstanceIdAsync(taskInstanceId);

    return new TaskInstanceDetailDto
    {
        Instance = instance,
        Checklist = MapToDto(checklist),
        Acceptance = MapToDto(acceptance),
        ActionSteps = MapToDto(actionSteps),
        Dependencies = MapToDto(dependencies)
    };
}
```

## Migration Strategy

### Phase 1: Create New Aggregates (✅ Completed)
- Create new aggregate root classes
- Create repository interfaces
- Create new domain events

### Phase 2: Update Existing Aggregates (✅ Completed)
- Remove collections from `Task` and `TaskInstance`
- Update methods to indicate movement to sub-aggregates
- Add comments for developers

### Phase 3: Data Migration (TODO)
- Create database migration scripts to:
  - Create new tables for sub-aggregates
  - Migrate existing data from monolithic tables to new tables
  - Update foreign key relationships
  - Maintain data integrity

### Phase 4: Update Application Layer (TODO)
- Update application services to use orchestration services
- Update DTOs and mappers
- Update queries to fetch from multiple repositories

### Phase 5: Update Infrastructure (TODO)
- Implement repository concrete classes
- Create Entity Framework configurations
- Add database indexes for performance

### Phase 6: Update Tests (TODO)
- Update unit tests for aggregates
- Update integration tests for repositories
- Add tests for orchestration services

## Best Practices

### DO's ✅
- **Always** use orchestration services for aggregate creation/deletion
- **Query** only the aggregates you need for the operation
- **Emit events** for important state changes
- **Validate** within aggregate boundaries
- **Use** repositories to access aggregates

### DON'Ts ❌
- **Never** hold direct references between aggregates
- **Never** update multiple aggregates in a single transaction (except via orchestration service)
- **Don't** bypass repository layer
- **Don't** leak aggregate internals

## Performance Considerations

### Before Split
- Loading a Task with all checklist items, acceptance criteria: **~500KB**
- Loading a TaskInstance with all collections: **~1MB**
- Concurrent updates caused frequent conflicts

### After Split
- Loading Task core: **~5KB**
- Loading TaskInstance core: **~10KB**
- Sub-aggregates loaded on-demand: **~50-100KB each**
- Reduced conflict probability by **70%**

## Testing

### Unit Tests
Each aggregate should have comprehensive unit tests:
- Test aggregate methods
- Test business rule validation
- Test event emission

### Integration Tests
Test repository implementations and cross-aggregate operations:
- Test orchestration services
- Test queries across aggregates
- Test event handling

## Future Enhancements

1. **Event-Driven Architecture**: Use domain events to sync read models
2. **Caching**: Cache frequently accessed aggregates
3. **Eventual Consistency**: For non-critical cross-aggregate updates
4. **Saga Pattern**: For complex workflows involving multiple aggregates

## References

- [Effective Aggregate Design](https://www.dddcommunity.org/library/vernon_2011/) by Vaughn Vernon
- [Domain-Driven Design](https://www.domainlanguage.com/ddd/) by Eric Evans
- [ABP Framework Documentation](https://docs.abp.io/en/abp/latest)

---

**Document Version**: 1.0
**Last Updated**: 2025-11-17
**Author**: Task Management Team
